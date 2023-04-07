using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GymAppCore.Models;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.User;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace GymAppInfrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly JwtSettings _jwtSettings;
    public IdentityService(UserManager<User> userManager, JwtSettings jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
    }
    
    public async Task<AuthenticationRegisterResult> Register(RegisterUserDto registerUserDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerUserDto.Email);

        if (existingUser is not null)
        {
            return new AuthenticationRegisterResult
            {
                Errors = new[] { "User with this email address already exist" }
            };
        }

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            UserName = registerUserDto.UserName,
            Email = registerUserDto.Email,
            Exercises = new(),
            Premium = new(),
            Friends = new(),
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
        var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
        var callbackUrl = "https://example.com/api/confirmEmail?userId=" + newUser.Id + "&code=" + emailToken;

        return new AuthenticationRegisterResult
        {
            Success = authResult.Success,
            Token = authResult.Token,
            callbackUrlEmailToken = callbackUrl
        };
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

    public async Task<bool> ConfirmEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new NullReferenceException("User not found");
        }
        var result = await _userManager.ConfirmEmailAsync(user, token);

        return result.Succeeded;
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
            Token = tokenHandler.WriteToken(token)
        };
    }
}