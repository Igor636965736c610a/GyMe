using GymAppCore.Models;
using GymAppInfrastructure.Dtos.Authorization;
using GymAppInfrastructure.Dtos.User;
using GymAppInfrastructure.ResetPasswordModel;
using Microsoft.AspNetCore.Http;

namespace GymAppInfrastructure.IServices;

public interface IIdentityService
{
    Task<AuthenticationRegisterResult> Register(RegisterUserDto registerUserDto, Func<string, string, string> generateCallbackToken, Func<string, string, string> resetPassword);
    Task CreateExternalUser(ResetPassword model);
    Task<AuthenticationLoginResult> Login(LoginUserDto loginUserDto);
    Task<bool> ConfirmEmail(string userId, string code);
    Task<ResetPasswordResult> ResetPassword(ResetPassword model);
}