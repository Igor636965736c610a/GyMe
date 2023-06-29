using GymAppCore.Models;
using GymAppInfrastructure.Dtos.User;
using Microsoft.AspNetCore.Http;

namespace GymAppInfrastructure.IServices;

public interface IIdentityService
{
    Task<AuthenticationRegisterResult> Register(RegisterUserDto registerUserDto, Func<string, string, string> generateCallbackToken);
    Task<AuthenticationLoginResult> Login(LoginUserDto loginUserDto);
    Task<bool> ConfirmEmail(string userId, string code);
}