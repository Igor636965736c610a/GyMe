using GymAppCore.Models;
using GymAppInfrastructure.Dtos.User;

namespace GymAppInfrastructure.IServices;

public interface IIdentityService
{
    Task<AuthenticationRegisterResult> Register(RegisterUserDto registerUserDto);
    Task<AuthenticationLoginResult> Login(LoginUserDto loginUserDto);
    Task<bool> ConfirmEmail(string userId, string token);
}