using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using FluentEmail.Core;
using FluentEmail.Mailgun;
using GymAppCore.IRepo;
using GymAppCore.Models;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Authorization;
using GymAppInfrastructure.Dtos.User;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Options;
using GymAppInfrastructure.ResetPasswordModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    private readonly SignInManager<User> _signInManager;
    private readonly JwtSettings _jwtSettings;
    private readonly EmailOptions _emailOptions;
    private readonly IUserRepo _userRepo;
    public IdentityService(UserManager<User> userManager, JwtSettings jwtSettings, IOptionsSnapshot<EmailOptions> emailOptions, IUserRepo userRepo, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
        _emailOptions = emailOptions.Value;
        _userRepo = userRepo;
        _signInManager = signInManager;
    }
    
    public async Task<AuthenticationRegisterResult> Register(RegisterUserDto registerUserDto, Func<string, string, string> generateCallbackToken, Func<string, string, string> resetPassword)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerUserDto.Email);

        if (existingUser is not null)
        {
            if (existingUser.AccountProvider == "App")
            {
                return new AuthenticationRegisterResult
                {
                    Errors = new[] { "User with this email address already exist" }
                };
            }
            else
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var callback = resetPassword(token, existingUser.Email);
                var resetSentResult = await SendEmail(callback, existingUser.Email);
                if (!resetSentResult)
                {
                    return new AuthenticationRegisterResult
                    {
                        Success = false,
                        Errors = new List<string> { "Something went wrong, try different login options" }
                    };
                }
                else
                {
                    return new AuthenticationRegisterResult
                    {
                        Success = true,
                        Messages = new List<string> { $"You are already registered by {existingUser.AccountProvider}. We have sent an email to your gmail to set your password" }
                    };
                }
            }
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
            Friends = new(),
            InverseFriends = new (),
            SendFriendRequests = new (),
            RecipientFriendRequests = new()
        };
        
        var createdUser = await _userManager.CreateAsync(newUser, registerUserDto.Password);

        if (!createdUser.Succeeded)
        {
            return new AuthenticationRegisterResult()
            {
                Errors = createdUser.Errors.Select(x => x.Description)
            };
        }
        
        var authResult =  GenerateAuthenticationResultForUser(newUser);

        var emailBody = "Please confirm your address <a href=\"#URL#\"> Click here </a> ";
        var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
        var callbackUrl = generateCallbackToken(newUser.Id.ToString(), emailToken);
        var body = emailBody.Replace("#URL#",
            callbackUrl);

        var result = await SendEmail(body, newUser.Email);
        if (!result)
        {
            return new AuthenticationRegisterResult
            {
                UserId = newUser.Id,
                Success = authResult.Success,
                Token = authResult.Token,
                Errors = new List<string> { "email verification link has not been sent" }
            };
        }

        return new AuthenticationRegisterResult
        {
            UserId = newUser.Id,
            Success = authResult.Success,
            Token = authResult.Token,
            Messages = new List<string> { "email verification link has been sent" }
        };
    }

    public Task CreateExternalUser(ResetPassword model)
    {
        throw new NotImplementedException();
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

    public async Task<ResetPasswordResult> ResetPassword(ResetPassword model)
    {
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser is null)
            throw new InvalidOperationException();
        var resetPassResult = await _userManager.ResetPasswordAsync(existingUser, model.Token, model.Password);
        if (!resetPassResult.Succeeded)
            return new ResetPasswordResult()
            {
                Success = false,
                Errors = resetPassResult.Errors.Select(x => x.Description)
            };
        existingUser.AccountProvider = "App";
        await _userRepo.Update(existingUser);
        return new ResetPasswordResult()
        {
            Success = true
        };
    }

    private AuthenticationLoginResult GenerateAuthenticationResultForUser(User user)
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
                new Claim("id", user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthenticationLoginResult
        {
            Success = true,
            Token = tokenHandler.WriteToken(token),
            UserId = user.Id
        };
    }

    private async Task<bool> SendEmail(string body, string email)
    {
        var sender = new MailgunSender(
            _emailOptions.DomainName,
            _emailOptions.ApiKey);
        
        Email.DefaultSender = sender;
        var emailToSend = Email
            .From(_emailOptions.From)
            .To(email)
            .Subject("Email Verification ")
            .Body(body);

        var response = await emailToSend.SendAsync();

        return response.Successful;
        //var options = new RestClientOptions("https://api.mailgun.net/v3")
        //{
        //    Authenticator = new HttpBasicAuthenticator("api",  "41bffed40acd5e661cba79b9a4dc477e-e5475b88-df79e86f") //"41bffed40acd5e661cba79b9a4dc477e-e5475b88-df79e86f"
        //};
        //RestClient client = new RestClient(options);
        //    
        //var request = new RestRequest("", Method.Post);

        //request.AddParameter("domain",
        //    "https://api.mailgun.net/v3/sandbox3b809dd3c13d4c7aa661ced28d9de67b.mailgun.org");
        //request.Resource = "{domain}/messages";
        //request.AddParameter("from", "Igor Miekina <igormiekina@sandbox3b809dd3c13d4c7aa661ced28d9de67b.mailgun.org>");
        //request.AddParameter("to", email);
        //request.AddParameter("subject", "Email Verification ");
        //request.AddParameter("text", body);
        //request.Method = Method.Post;

        //var response = await client.ExecuteAsync(request);

        //return response.IsSuccessful;
    }
}