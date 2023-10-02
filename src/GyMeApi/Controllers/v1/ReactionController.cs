using GymAppApi.Routes.v1;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Models.ReactionsAndComments;
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

    [HttpPost(ApiRoutes.Reaction.AddReaction)]
    public async Task<IActionResult> AddReaction([FromBody]PostReactionDto postReactionDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _reactionService.AddEmojiReaction(postReactionDto.SimpleExerciseId, postReactionDto.ReactionType);

        return Ok();
    }

    [HttpPost(ApiRoutes.Reaction.SetImageReaction)]
    [RequestSizeLimit(1000*1024)]
    public async Task<IActionResult> SetImageReaction([FromForm]IFormFile image)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _reactionService.SetImageReaction(image);

        return Ok();
    }

    [HttpGet(ApiRoutes.Reaction.GetReactions)]
    public async Task<IActionResult> GetReactions([FromQuery]string simpleExerciseId, [FromQuery]int page, [FromQuery]int size, [FromQuery]ReactionType? optionalReactionTypeToSort) 
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var reactions = await _reactionService.GetReactions(Guid.Parse(simpleExerciseId), page, size, optionalReactionTypeToSort);

        return Ok(reactions);
    }

    [HttpGet(ApiRoutes.Reaction.GetSpecificReactionsCount)]
    public async Task<IActionResult> GetSpecificReactionsCount([FromQuery]string simpleExerciseId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var reactionsCount = await _reactionService.GetSpecificReactionsCount(Guid.Parse(simpleExerciseId));

        return Ok(reactionsCount);
    }

    [HttpDelete(ApiRoutes.Reaction.RemoveReaction)]
    public async Task<IActionResult> RemoveReaction([FromRoute]string reactionId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _reactionService.RemoveReaction(Guid.Parse(reactionId));

        return Ok();
    }
}