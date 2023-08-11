using System.Drawing;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GymAppApi.Controllers.HelperAttributes;
using GymAppApi.Routes.v1;
using GymAppInfrastructure.Dtos.User;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using SixLabors.ImageSharp.Formats.Jpeg;


namespace GymAppApi.Controllers.v1;

[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly IAccountService _accountService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AccountController(IIdentityService identityService, IAccountService accountService, IWebHostEnvironment webHostEnvironment)
    {
        _identityService = identityService;
        _accountService = accountService;
        _webHostEnvironment = webHostEnvironment;
    }

    [AllowAnonymous]
    [HttpPost(ApiRoutes.Account.Register)]
    public async Task<IActionResult> Register([FromBody]RegisterUserDto registerUserDto, IFormFile? pictureFile)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var profilePicture = await ValidateAndScaleProfilePicture(pictureFile, 200, 200);

        Func<string, string, string> createCallbackUrl = (userIdParam, codeParam)
            => Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail", "Account", new
            {
                userId = userIdParam,
                code = codeParam
            });

        var result = await _identityService.Register(registerUserDto, profilePicture, createCallbackUrl);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result);
    }
    
    [AllowAnonymous]
    [HttpPost(ApiRoutes.Account.SendResetPasswordToken)]
    public async Task<IActionResult> SendResetPasswordToken([FromQuery] string email)
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
    public async Task<IActionResult> ResetPassword([FromBody] ResetPassword model)
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

    [Authorize(Policy = "SSO")]
    [HttpPut(ApiRoutes.Account.UpdateUser)]
    public async Task<IActionResult> UpdateUser([FromBody] PutUserDto putUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _accountService.Update(putUserDto);

        return Ok();
    }

    [Authorize(Policy = "SSO")]
    [SkipValidAccountCheck]
    [HttpPost(ApiRoutes.Account.ActivateUser)]
    public async Task<IActionResult> ActivateUser([FromQuery] string userName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        await _identityService.ActivateUser(userName);

        return Ok();
    }

    [Authorize(Policy = "SSO")]
    [HttpDelete(ApiRoutes.Account.RemoveUser)]
    public async Task<IActionResult> RemoveUser()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _accountService.Remove();

        return Ok();
    }

    [Authorize(Policy = "SSO")]
    [SkipValidAccountCheck]
    [HttpGet(ApiRoutes.Account.GetAccountInformation)]
    public async Task<IActionResult> GetAccountInformation()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var accountInf = await _accountService.GetInf();

        return Ok(accountInf);
    }
    private async Task<byte[]> ValidateAndScaleProfilePicture(IFormFile? pictureFile, int maxWidth, int maxHeight)
    {
        if (pictureFile is null || pictureFile.Length == 0)
        {
            var defaultImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "images/defaultProfilePicture.jpg");
            return await System.IO.File.ReadAllBytesAsync(defaultImagePath);
        }
        
        if (pictureFile.Length > 400 * 1024)
        {
            throw new ArgumentException("Picture size exceeds the allowed limit of 400 KB.");
        }

        using var image = await Image.LoadAsync(pictureFile.OpenReadStream());

        if (image.Width > maxWidth || image.Height > maxHeight)
        {
            using var resizedImage = ResizeImage(image, maxWidth, maxHeight);
            await using var memoryStream = new MemoryStream();
            await resizedImage.SaveAsync(memoryStream, new JpegEncoder());
            return memoryStream.ToArray();
        }

        await using (var memoryStream = new MemoryStream())
        {
            await image.SaveAsync(memoryStream, new JpegEncoder());
            return memoryStream.ToArray();
        }
    }

    private static Image ResizeImage(Image image, int maxWidth, int maxHeight)
    {
        double ratioX = (double)maxWidth / image.Width;
        double ratioY = (double)maxHeight / image.Height;
        double ratio = Math.Min(ratioX, ratioY);
        int newWidth = (int)(image.Width * ratio);
        int newHeight = (int)(image.Height * ratio);

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new SixLabors.ImageSharp.Size(newWidth, newHeight),
            Mode = ResizeMode.Max
        }));

        return image;
    }
}