using GymAppApi.BodyRequest.User;
using GymAppApi.Routes.v1;
using GymAppInfrastructure.Dtos.User;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Bcpg;

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
            
        var result = await _identityService.Register(registerUserDto, createCallbackUrl);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result);
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

        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));

        await _accountService.Remove(userId);

        return Ok();
    }

    [Authorize]
    [HttpGet(ApiRoutes.Account.GetAccountInformation)]
    public async Task<IActionResult> GetAccountInformation()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));

        var accountInf = await _accountService.GetInf(userId);

        return Ok(accountInf);
    }
}