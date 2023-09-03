using GymAppApi.Routes.v1;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Models.InternalManagement;
using GymAppInfrastructure.Services;
using GymAppInfrastructure.Services.InternalManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymAppApi.Controllers.v1;

[Authorize(Policy = "SSO")]
[Route("[controller]")]
public class UserFeedbackController : ControllerBase
{
    private readonly OpinionService _opinionService;
    private readonly IUserContextService _userContextService;

    public UserFeedbackController(OpinionService opinionService, IUserContextService userContextService)
    {
        _opinionService = opinionService;
        _userContextService = userContextService;
    }

    [HttpPost(ApiRoutes.UserFeedback.SendOpinion)]
    public async Task<IActionResult> SendOpinionMessage([FromBody]OpinionRequestBody opinionRequestBody)
    {
        if (!ModelState.IsValid)
            return BadRequest("InvalidData");
        
        var email = _userContextService.Email;
        var opinion = new Opinion()
        {
            Message = opinionRequestBody.Message,
            Email = email
        };
        await _opinionService.Add(opinion);

        return Ok("Thank you for your opinion!");
    }
}