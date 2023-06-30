using System.Security.Claims;
using GymAppApi.BodyRequest.User;
using GymAppApi.Routes.v1;
using GymAppInfrastructure.Dtos.User;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.ResetPasswordModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GymAppApi.Controllers.v1;

[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IAccountService _accountService;

    public AccountController(IIdentityService identityService, IAccountService accountService)
    {
        _identityService = identityService;
        _accountService = accountService;
    }

    [AllowAnonymous]
    [HttpPost(ApiRoutes.Account.Register)]
    public async Task<IActionResult> Register([FromBody]RegisterUserDto registerUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Func<string, string, string> createCallbackUrl = (userIdParam, codeParam)
            => Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail", "Account", new
            {
                userId = userIdParam,
                code = codeParam
            });
            
        Func<string, string, string> resetPassword = (token, email)
            => Request.Scheme + "://" + Request.Host + Url.Action("ResetPassword", "Account", new
        {
            token = token,
            email = email
        });
        
        var result = await _identityService.Register(registerUserDto, createCallbackUrl, resetPassword);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(string token, string email)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var model = new ResetPassword { Token = token, Email = email };

        return Ok(new
        {
            model
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPassword model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _identityService.ResetPassword(model);

        if (!response.Success)
            return BadRequest(response.Errors);
        return Ok(response);
    }
    
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Account.ConfirmEmail)]
    public async Task<IActionResult> ConfirmEmail([FromQuery]string userId,[FromQuery] string code)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
        {
            return BadRequest("Invalid token or user id");
        }

        var result = await _identityService.ConfirmEmail(userId, code);
        if (!result)
        {
            return BadRequest("Failed to confirm email");
        }
        
        return Ok("Email confirmed successfully");
    }
    
    [AllowAnonymous]
    [HttpPost(ApiRoutes.Account.Login)]
    public async Task<IActionResult> Login([FromBody]LoginUserDto loginUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await _identityService.Login(loginUserDto);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }
        
        return Ok(result);
    }
    
    public IActionResult ExternalLoginFacebook()
    {
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("", "Account")
        };
        return Challenge(authenticationProperties, FacebookDefaults.AuthenticationScheme);
    }
    
    //[AllowAnonymous]
    //public async Task<IActionResult> ExternalLoginCallback()
    //{
    //    var authenticateResult = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
    //    if (!authenticateResult.Succeeded)
    //    {
    //        return RedirectToAction("Login", "Account");
    //    }
    //    
    //    await _identityService CreateExternalUser();
    //    var userId = authenticateResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
    //    var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
    //    var name = authenticateResult.Principal.FindFirstValue(ClaimTypes.Name);
    //    var surname = authenticateResult.Principal.FindFirstValue(ClaimTypes.Surname);
//
    //    // Zaimplementuj swoją logikę rejestracji użytkownika
//
    //    // Przekieruj do strony po zalogowaniu
    //    return Ok(); //RedirectToAction("Index", "Home");
    //}

    [Authorize]
    [HttpPut(ApiRoutes.Account.UpdateUser)]
    public async Task<IActionResult> UpdateUser([FromBody] PutUserBody putUserBody)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));

        PutUserDto putUserDto = new()
        {
            UserName = putUserBody.UserName,
            FirstName = putUserBody.FirstName,
            LastName = putUserBody.LastName,
            PrivateAccount = putUserBody.PrivateAccount
        };

        await _accountService.Update(userId, putUserDto);

        return Ok();
    }

    [Authorize]
    [HttpDelete(ApiRoutes.Account.RemoveUser)]
    public async Task<IActionResult> RemoveUser()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var jwtId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));

        await _accountService.Remove(jwtId);

        return Ok();
    }

    [Authorize]
    [HttpGet(ApiRoutes.Account.GetAccountInformation)]
    public async Task<IActionResult> GetAccountInformation()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var jwtId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));

        var accountInf = await _accountService.GetInf(jwtId);

        return Ok(accountInf);
    }
}