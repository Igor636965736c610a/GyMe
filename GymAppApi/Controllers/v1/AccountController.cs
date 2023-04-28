using GymAppApi.BodyRequest.User;
using GymAppApi.Routes.v1;
using GymAppInfrastructure.Dtos.User;
using GymAppInfrastructure.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymAppApi.Controllers.v1;

[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IEmailConfirmationService _emailConfirmationService;
    private readonly IAccountService _accountService;

    public AccountController(IIdentityService identityService, IEmailConfirmationService emailConfirmationService, IAccountService accountService)
    {
        _identityService = identityService;
        _emailConfirmationService = emailConfirmationService;
        _accountService = accountService;
    }

    [HttpPost(ApiRoutes.Account.Register)]
    public async Task<IActionResult> Register([FromBody]RegisterUserDto registerUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await _identityService.Register(registerUserDto);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        await _emailConfirmationService.SendConfirmationEmailAsync(registerUserDto.Email, result.callbackUrlEmailToken);

        return Ok(result);
    }
    
    [HttpGet(ApiRoutes.Account.ConfirmEmail)]
    public async Task<IActionResult> ConfirmEmail([FromQuery]string userId,[FromQuery] string token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        {
            return BadRequest("Invalid token or user id");
        }

        var result = await _identityService.ConfirmEmail(userId, token);
        if (!result)
        {
            return BadRequest("Failed to confirm email");
        }
        
        return Ok("Email confirmed successfully");
    }
    
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
    [HttpGet(ApiRoutes.Account.GetAccountInformation)]
    public async Task<IActionResult> GetAccountInformation()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));

        var accountInf = await _accountService.GetAccountInf(userId);

        return Ok(accountInf);
    }
}