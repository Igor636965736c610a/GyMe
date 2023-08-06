using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using FluentEmail.Core;
using FluentEmail.Mailgun;
using GymAppCore.IRepo;
using GymAppCore.Models;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.User;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Options;
using GymAppInfrastructure.Results;
using GymAppInfrastructure.Results.Authorization;
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

namespace GymAppInfrastructure.Services;

internal class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly JwtSettings _jwtSettings;
    private readonly EmailOptions _emailOptions;
    private readonly IUserRepo _userRepo;
    private readonly IUserContextService _userContextService;
    
    public IdentityService(
        UserManager<User> userManager, JwtSettings jwtSettings, 
        IOptionsSnapshot<EmailOptions> emailOptions, 
        IUserRepo userRepo, SignInManager<User> signInManager, 
        ApplicationFacebookOptions applicationFacebookOptions,
        IUserContextService userContextService)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
        _emailOptions = emailOptions.Value;
        _userRepo = userRepo;
        _userContextService = userContextService;
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
            var result = await SendEmail(body, subject, existingUser.Email);
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

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            UserName = registerUserDto.UserName,
            PrivateAccount = registerUserDto.PrivateAccount,
            Email = registerUserDto.Email,
            EmailConfirmed = false,
            Exercises = new(),
            Premium = new(),
            Valid = true,
            Friends = new(),
            InverseFriends = new (),
            AccountProvider = nameof(AccountProviderOptions.App),
        };
        
        var createdUser = await _userManager.CreateAsync(newUser, registerUserDto.Password);

        if (!createdUser.Succeeded)
        {
            return new AuthenticationRegisterResult()
            {
                Success = false,
                Errors = createdUser.Errors.Select(x => x.Description)
            };
        }

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
            throw new InvalidOperationException("Something went wrong");

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
            PrivateAccount = true,
            Email = email,
            EmailConfirmed = true,
            Exercises = new(),
            Premium = new(),
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

    public async Task<bool> ConfirmEmail(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new NullReferenceException("User not found");
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);

        return result.Succeeded;
    }

    public async Task<ActivateUserResult> ActivateUser(string userName)
    {
        var userIdFromJwt = _userContextService.GetUserId;
        
        var user = await _userRepo.Get(userIdFromJwt);
        if (user == null)
        {
            throw new NullReferenceException("User not found");
        }

        if (user.Valid)
            throw new InvalidOperationException("User is already Activate");

        if (userName.Length < 2)
            return new ActivateUserResult()
            {
                Success = false,
                Errors = new[] { "username must contain at least 2 characters" }
            };
        var userWithTheSameUsername = await _userRepo.Get(userName);
        if (userWithTheSameUsername is not null)
            throw new InvalidOperationException("User with this username already exist");

        user.UserName = userName;
        user.Valid = true;
        await _userRepo.Update(user);

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
            throw new InvalidOperationException("User does not exist");
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var body = $"Password reset token = {token}";
        var subject = "Reset Password token";

        var result = await SendEmail(body, subject, user.Email);

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

        var result = await SendEmail(body, subject, user.Email);
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
                new Claim("SSO", "SSO")
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private async Task<bool> SendEmail(string body, string subject, string? email)
    {
        var sender = new MailgunSender(
            _emailOptions.DomainName,
            _emailOptions.ApiKey);
        
        Email.DefaultSender = sender;
        var emailToSend = Email
            .From(_emailOptions.From)
            .To(email)
            .Subject(subject)
            .Body(body);

        var response = await emailToSend.SendAsync();

        return response.Successful;
    }
}