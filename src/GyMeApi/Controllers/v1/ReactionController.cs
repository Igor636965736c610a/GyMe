using GymAppApi.Routes.v1;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Models.ReactionsAndComments.BodyRequest;
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

        await _reactionService.AddEmojiReaction(postEmojiReaction.SimpleExerciseId, postEmojiReaction.Emoji);

        return Ok();
    }

    [HttpPost(ApiRoutes.Reaction.AddImageReaction)]
    [RequestSizeLimit(1000*1024)]
    public async Task<IActionResult> AddImageReaction([FromForm] IFormFile image, [FromQuery] string simpleExerciseId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var simpleExerciseIdGuid = Guid.Parse(simpleExerciseId);

        await _reactionService.AddImageReaction(simpleExerciseIdGuid, image);

        return Ok();
    }
    
    
}