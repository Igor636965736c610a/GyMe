using GyMeApplication.Models.Account;
using GyMeApplication.Models.User;
using GyMeApplication.Results;
using GyMeApplication.Results.Authorization;
using GyMeCore.Models;
using Microsoft.AspNetCore.Http;

namespace GyMeApplication.IServices;

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