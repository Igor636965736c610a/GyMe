using GymAppInfrastructure.Dtos.User;
using GymAppInfrastructure.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GymAppApi.Controllers.v1;

[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IEmailConfirmationService _emailConfirmationService;

    public AccountController(IIdentityService identityService, IEmailConfirmationService emailConfirmationService)
    {
        _identityService = identityService;
        _emailConfirmationService = emailConfirmationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]RegisterUserDto registerUserDto)
    {
        var result = await _identityService.Register(registerUserDto);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        await _emailConfirmationService.SendConfirmationEmailAsync(registerUserDto.Email, result.callbackUrlEmailToken);

        return Ok(result);
    }
    
    [HttpGet("confirmEmail")]
    public async Task<IActionResult> ConfirmEmail([FromQuery]string userId, string token)
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
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginUserDto loginUserDto)
    {
        var result = await _identityService.Login(loginUserDto);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }
        
        return Ok(result);
    }
}