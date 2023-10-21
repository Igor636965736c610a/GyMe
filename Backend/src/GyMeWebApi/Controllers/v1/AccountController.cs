using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GyMeApplication.Models.User;
using GyMeApplication.IServices;
using GyMeApplication.Models.Account;
using GyMeApplication.Results;
using GyMeWebApi.Controllers.HelperAttributes;
using GyMeWebApi.Routes.v1;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using SixLabors.ImageSharp.Formats.Jpeg;
using Image = SixLabors.ImageSharp.Image;


namespace GyMeWebApi.Controllers.v1;

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

        var result = await _identityService.Register(registerUserDto, createCallbackUrl);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result);
    }

    [Authorize(Policy = "AppSys")]
    [RequestSizeLimit(1000*1024)]
    [HttpPost(ApiRoutes.Account.SetProfilePicture)]
    public async Task<IActionResult> SetProfilePicture([FromForm]IFormFile image)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _accountService.SetUserProfile(image);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost(ApiRoutes.Account.SendResetPasswordToken)]
    public async Task<IActionResult> SendResetPasswordToken([FromQuery]string email)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _identityService.SendResetPasswordToken(email);

        if(!response)
            return StatusCode(500,"Password reset token has not been sent");
        
        return Ok("Password reset token has been sent");
    }
    
    [AllowAnonymous]
    [HttpPost(ApiRoutes.Account.ResetPassword)]
    public async Task<IActionResult> ResetPassword([FromBody]ResetPassword model)
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
    public async Task<IActionResult> ConfirmEmail([FromQuery]Guid userId,[FromQuery]string code)
    {
        if (string.IsNullOrEmpty(code))
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
    
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Account.ExternalLogin)]
    public IActionResult ExternalLoginFacebook()
    {
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(HandleFacebookLoginCallback), "Account"),
            Items =
            {
                { "LoginProvider", "Facebook" }
            }
        };
        return Challenge(authenticationProperties, FacebookDefaults.AuthenticationScheme);
    }

    [AllowAnonymous]
    [HttpGet(ApiRoutes.Account.HandleExternalLogin)]
    public async Task<IActionResult> HandleFacebookLoginCallback()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
        if (!authenticateResult.Succeeded)
        {
            return RedirectToAction("Login", "Account");
        }

        var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
        var name = authenticateResult.Principal.FindFirstValue(ClaimTypes.Name);

        var result = await _identityService.ExternalLogin(email, name);
        
        return Ok(result);
    }

    [Authorize(Policy = "AppSys")]
    [HttpPut(ApiRoutes.Account.UpdateUser)]
    public async Task<IActionResult> UpdateUser([FromBody]PutUserDto putUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _accountService.Update(putUserDto);

        return Ok(user);
    }

    [Authorize(Policy = "AppSys")]
    [SkipValidAccountCheck]
    [HttpPost(ApiRoutes.Account.ActivateAccount)]
    public async Task<IActionResult> ActivateAccount([FromBody]ActivateAccountModel activateAccountModel) 
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await _identityService.ActivateUser(activateAccountModel);

        return Ok(result);
    }

    [Authorize(Policy = "AppSys")]
    [HttpDelete(ApiRoutes.Account.RemoveAccount)]
    public async Task<IActionResult> RemoveAccount()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _accountService.Remove();

        return Ok();
    }

    [Authorize(Policy = "AppSys")]
    [SkipValidAccountCheck]
    [HttpGet(ApiRoutes.Account.GetAccountInformation)]
    public async Task<IActionResult> GetAccountInformation()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var accountInf = await _accountService.GetInf();

        return Ok(accountInf);
    }
}