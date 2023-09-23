using GymAppApi.Routes.v1;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Models.ReactionsAndComments.BodyRequest.BodyRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymAppApi.Controllers.v1;

[Authorize(Policy = "SSO")]
[Route("[controller]")]
public class ReactionController : ControllerBase
{
    private readonly IReactionService _reactionService;

    public ReactionController(IReactionService reactionService)
    {
        _reactionService = reactionService;
    }

    [HttpPost(ApiRoutes.Reaction.AddEmojiReaction)]
    public async Task<IActionResult> AddEmojiReaction([FromBody]PostEmojiReaction postEmojiReaction)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _reactionService.AddEmojiReaction(postEmojiReaction.SimpleExerciseId, postEmojiReaction.ReactionType);

        return Ok();
    }

    [HttpPost(ApiRoutes.Reaction.SetImageReaction)]
    [RequestSizeLimit(1000*1024)]
    public async Task<IActionResult> SetImageReaction([FromForm] IFormFile image)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _reactionService.SetImageReaction(image);

        return Ok();
    }
    
    
}