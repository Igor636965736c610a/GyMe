using GyMeCore.Models;
using GyMeInfrastructure.Models.Account;
using GyMeInfrastructure.Models.User;
using GyMeInfrastructure.Results;
using GyMeInfrastructure.Results.Authorization;
using Microsoft.AspNetCore.Http;

namespace GyMeInfrastructure.IServices;

public interface IIdentityService
{
    Task<AuthenticationRegisterResult> Register(RegisterUserDto registerUserDto, Func<string, string, string> generateCallbackToken);
    Task<AuthenticationLoginResult> Login(LoginUserDto loginUserDto);
    Task<AuthenticationLoginResult> ExternalLogin(string? email, string? nameSurname);
    Task<bool> ConfirmEmail(string userId, string code);
    Task<ActivateUserResult> ActivateUser(ActivateAccountModel activateAccountModel);
    Task<bool> SendResetPasswordToken(string email);
    Task<ResetPasswordResult> ResetPassword(ResetPassword model);
}