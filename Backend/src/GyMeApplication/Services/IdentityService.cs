using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Transactions;
using FluentEmail.Core;
using FluentEmail.Mailgun;
using GyMeApplication.IServices;
using GyMeApplication.Models.Account;
using GyMeApplication.Models.User;
using GyMeApplication.Options;
using GyMeApplication.Results;
using GyMeApplication.Results.Authorization;
using GyMeApplication.Services.InternalManagement;
using GyMeCore.IRepo;
using GyMeCore.Models;
using GyMeCore.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using RestSharp;
using RestSharp.Authenticators;

namespace GyMeApplication.Services;

internal class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly IUserRepo _userRepo;
    private readonly IUserContextService _userContextService;
    private readonly IGyMeResourceService _gyMeResourceService;
    private readonly IEmailSender _emailSender;

    public IdentityService(
        UserManager<User> userManager, JwtSettings jwtSettings,
        IUserRepo userRepo, IUserContextService userContextService,
        IGyMeResourceService gyMeResourceService, IEmailSender emailSender)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
        _userRepo = userRepo;
        _userContextService = userContextService;
        _gyMeResourceService = gyMeResourceService;
        _emailSender = emailSender;
    }
    
    public async Task<AuthenticationRegisterResult> Register(RegisterUserDto registerUserDto, Func<string, string, string> generateCallbackToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerUserDto.Email);

        if (existingUser is not null)
        {
            if (existingUser.AccountProvider.Contains("App"))
            {
                return new AuthenticationRegisterResult
                { 
                    Success = false,
                    Errors = new[] { "User with this email address already exist" }
                };
            }
            
            var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
            var body =
                $"Registration verification token = {token}";
            var subject = "verify registration";
            var result = await _emailSender.SendEmail(body, subject, existingUser.Email);
            if(!result)
                return new AuthenticationRegisterResult
                {
                    Success = false,
                    Errors = new[] { "You are already registered into our app by facebook. We have sent a password token to your e-mail address but something went wrong while sending email. " +
                                     "PLeas try later" }
                };
            return new AuthenticationRegisterResult
            {
                Success = true,
                Messages = new[] { "You are already registered into our app by facebook. We have sent a password token to your e-mail address" }
            };
        }
        var id = Guid.NewGuid();

        var newUser = new User
        {
            Id = id,
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            UserName = registerUserDto.UserName,
            Email = registerUserDto.Email,
            EmailConfirmed = false,
            Exercises = new(),
            Valid = true,
            Premium = false,
            ExtendedUser = new ExtendedUser()
            {
                PrivateAccount = registerUserDto.PrivateAccount,
                Gender = registerUserDto.GenderDto.ToStringFast(),
                ProfilePictureUrl = _gyMeResourceService.GenerateUrlToPhoto(id.ToString(), id.ToString()) + ".jpg",
                Description = registerUserDto.Description,
            },
            Friends = new(),
            InverseFriends = new (),
            AccountProvider = nameof(AccountProviderOptions.App),
        };
        var createdUser = await _userManager.CreateAsync(newUser, registerUserDto.Password);
        if (!createdUser.Succeeded)
        {
            return new AuthenticationRegisterResult
            {
                Success = false,
                Errors = createdUser.Errors.Select(x => x.Description)
            };
        }
        _gyMeResourceService.SetDefaultProfilePicture(id.ToString());

        return await AuthenticateUser(newUser, generateCallbackToken);
    }

    public async Task<AuthenticationLoginResult> Login(LoginUserDto loginUserDto)
    {
        var user = await _userManager.FindByEmailAsync(loginUserDto.Email);

        if (user is null)
        {
            return new AuthenticationLoginResult
            {
                Errors = new[] { "User does not exist" }
            };
        }

        var userHasValidPassword = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);

        if (!userHasValidPassword)
        {
            return new AuthenticationLoginResult
            {
                Errors = new[] { "User/password combination is wrong" }
            };
        }

        return GenerateAuthenticationResultForUser(user);
    }

    public async Task<AuthenticationLoginResult> ExternalLogin(string? email, string? nameSurname)
    {
        if (email is null || nameSurname is null)
            throw new InvalidProgramException("Something went wrong");

        var user = await _userManager.FindByEmailAsync(email);
        if (user is not null)
        {
            if (!user.AccountProvider.Contains(nameof(AccountProviderOptions.Fb)))
                user.AccountProvider += " " + nameof(AccountProviderOptions.Fb);
            
            return GenerateAuthenticationResultForUser(user);
        }

        var name = nameSurname.Split(" ").First();
        var surname = nameSurname.Split(" ").Last();

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            FirstName = name,
            LastName = surname,
            UserName = Guid.NewGuid().ToString(),
            Email = email,
            EmailConfirmed = true,
            Exercises = new(),
            Valid = false,
            Friends = new(),
            InverseFriends = new(),
            AccountProvider = nameof(AccountProviderOptions.Fb)
        };
        
        var createdUser = await _userManager.CreateAsync(newUser);

        if (!createdUser.Succeeded)
        {
            return new AuthenticationLoginResult()
            {
                Errors = createdUser.Errors.Select(x => x.Description)
            };
        }

        return GenerateAuthenticationResultForUser(newUser);
    }

    public async Task<bool> ConfirmEmail(Guid userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            throw new NullReferenceException("User not found");
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);

        return result.Succeeded;
    }

    public async Task<ActivateUserResult> ActivateUser(ActivateAccountModel activateAccountModel)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var user = await _userRepo.Get(userIdFromJwt);
        if (user is null)
        {
            throw new NullReferenceException("User not found");
        }

        if (user.Valid)
            throw new InvalidOperationException("User is already Activate");

        var userWithTheSameUsername = await _userRepo.Get(activateAccountModel.UserName);
        if (userWithTheSameUsername is not null)
            throw new InvalidOperationException("User with this username already exist");

        var imageUrl = _gyMeResourceService.GenerateUrlToPhoto(user.Id.ToString(), user.Id.ToString()) + ".jpg";
        var extendedUser = new ExtendedUser()
        {
            Gender = activateAccountModel.GenderDto.ToStringFast(),
            PrivateAccount = activateAccountModel.PrivateAccount,
            ProfilePictureUrl = imageUrl,
            Description = activateAccountModel.Description,
            User = user
        };

        user.UserName = activateAccountModel.UserName;
        user.Valid = true;
        user.ExtendedUser = extendedUser;

        await _userRepo.Update(user);
        _gyMeResourceService.SetDefaultProfilePicture(user.Id.ToString());

        var token = GenerateToken(user);

        return new ActivateUserResult()
        {
            Token = token,
            Success = true,
        };
    }

    public async Task<bool> SendResetPasswordToken(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            throw new NullReferenceException("User does not exist");
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var body = $"Password reset token = {token}";
        var subject = "Reset Password token";

        var result = await _emailSender.SendEmail(body, subject, user.Email);

        return result;
    }

    public async Task<ResetPasswordResult> ResetPassword(ResetPassword model)
    {
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser is null)
            throw new InvalidOperationException("User does not exist");
        var resetPassResult = await _userManager.ResetPasswordAsync(existingUser, model.Token, model.Password);
        if (!resetPassResult.Succeeded)
            return new ResetPasswordResult()
            {
                Success = false,
                Errors = resetPassResult.Errors.Select(x => x.Description)
            };
        if (!existingUser.AccountProvider.Contains(nameof(AccountProviderOptions.App)))
            existingUser.AccountProvider += " " + nameof(AccountProviderOptions.App);
        await _userRepo.Update(existingUser);
        return new ResetPasswordResult()
        {
            Success = true
        };
    }

    private async Task<AuthenticationRegisterResult> AuthenticateUser(User user, Func<string, string, string> generateCallbackToken)
    {
        var token = GenerateToken(user);

        var emailBody = "Please confirm your address <a href=\"#URL#\"> Click here </a> ";
        var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var callbackUrl = generateCallbackToken(user.Id.ToString(), emailToken);
        var body = emailBody.Replace("#URL#",
            callbackUrl);
        var subject = "Email Verification ";

        var result = await _emailSender.SendEmail(body, subject, user.Email);
        if (!result)
        {
            return new AuthenticationRegisterResult
            {
                UserId = user.Id,
                Success = true,
                Token = token,
                Errors = new List<string> { "email verification link has not been sent" }
            };
        }

        return new AuthenticationRegisterResult
        {
            UserId = user.Id,
            Success = true,
            Token = token,
            Messages = new List<string> { "email verification link has been sent" }
        };
    }

    private AuthenticationLoginResult GenerateAuthenticationResultForUser(User user)
    {
        var token = GenerateToken(user);

        return new AuthenticationLoginResult
        {
            Success = true,
            ValidUser = user.Valid,
            Token = token,
            UserId = user.Id
        };
    }

    private string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim("validAccount", user.Valid.ToString()),
                new Claim("EmailConfirmed", user.EmailConfirmed.ToString()),
                new Claim("AppSys", "AppSys")
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}