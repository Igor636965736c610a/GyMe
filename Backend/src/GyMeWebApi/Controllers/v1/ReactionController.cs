using GyMeApplication.IServices;
using GyMeApplication.Models.ReactionsAndComments;
using GyMeApplication.Models.ReactionsAndComments.BodyRequest.BodyRequest;
using GyMeWebApi.Routes.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GyMeWebApi.Controllers.v1;

[Authorize(Policy = "AppSys")]
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

        var reactionId = await _reactionService.AddReaction(postReactionDto.SimpleExerciseId, postReactionDto.ReactionType);
        
        var location = Url.Action("GetReaction",new { id = reactionId })!;

        return Created(location, reactionId.ToString());
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
    
    [HttpGet(ApiRoutes.Reaction.GetReaction)]
    public async Task<IActionResult> GetReaction([FromRoute]Guid id) 
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var reactions = await _reactionService.GetReaction(id);

        return Ok(reactions);
    }

    [HttpGet(ApiRoutes.Reaction.GetReactions)]
    public async Task<IActionResult> GetReactions([FromQuery]Guid simpleExerciseId, [FromQuery]int page, [FromQuery]int size, [FromQuery]ReactionType? optionalReactionTypeToSort) 
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var reactions = await _reactionService.GetReactions(simpleExerciseId, page, size, optionalReactionTypeToSort);

        return Ok(reactions);
    }

    [HttpGet(ApiRoutes.Reaction.GetSpecificReactionsCount)]
    public async Task<IActionResult> GetSpecificReactionsCount([FromQuery]Guid simpleExerciseId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var reactionsCount = await _reactionService.GetSpecificReactionsCount(simpleExerciseId);

        return Ok(reactionsCount);
    }

    [HttpDelete(ApiRoutes.Reaction.RemoveReaction)]
    public async Task<IActionResult> RemoveReaction([FromRoute]Guid reactionId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _reactionService.RemoveReaction(reactionId);

        return Ok();
    }
}