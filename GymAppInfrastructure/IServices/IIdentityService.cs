using GymAppCore.Models;
using GymAppInfrastructure.Dtos.User;
using GymAppInfrastructure.Results;
using GymAppInfrastructure.Results.Authorization;
using Microsoft.AspNetCore.Http;

namespace GymAppInfrastructure.IServices;

public interface IIdentityService
{
    Task<AuthenticationRegisterResult> Register(RegisterUserDto registerUserDto, Func<string, string, string> generateCallbackToken);
    Task<AuthenticationLoginResult> Login(LoginUserDto loginUserDto);
    Task<AuthenticationLoginResult> ExternalLogin(string? email, string? nameSurname);
    Task<bool> ConfirmEmail(string userId, string code);
    Task ActivateUser(Guid jwtId, string userName);
    Task<bool> SendResetPasswordToken(string email);
    Task<ResetPasswordResult> ResetPassword(ResetPassword model);
}