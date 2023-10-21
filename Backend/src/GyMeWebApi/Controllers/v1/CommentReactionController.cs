using GyMeApplication.IServices;
using GyMeApplication.Models.ReactionsAndComments;
using GyMeApplication.Models.ReactionsAndComments.BodyRequest;
using GyMeWebApi.Routes.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GyMeWebApi.Controllers.v1;

[Authorize(Policy = "AppSys")]
[Route("[controller]")]
public class CommentReactionController : ControllerBase
{
    private readonly ICommentReactionService _commentReactionService;

    public CommentReactionController(ICommentReactionService commentReactionService)
    {
        _commentReactionService = commentReactionService;
    }

    [HttpPost(ApiRoutes.CommentReaction.AddCommentReaction)]
    public async Task<IActionResult> AddCommentReaction([FromBody]PostCommentReactionDto postCommentReactionDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var commentReactionId = await _commentReactionService.AddCommentReaction(postCommentReactionDto);
        
        var location = Url.Action("GetCommentReaction",new { id = commentReactionId })!;

        return Created(location, commentReactionId.ToString());
    }
    
    [HttpGet(ApiRoutes.CommentReaction.GetCommentReaction)]
    public async Task<IActionResult> GetCommentReaction([FromRoute]Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var commentReactions =
            await _commentReactionService.GetCommentsReaction(id);

        return Ok(commentReactions);
    }

    [HttpGet(ApiRoutes.CommentReaction.GetCommentReactions)]
    public async Task<IActionResult> GetCommentReactions([FromQuery]Guid commentId, [FromQuery]CommentReactionType? commentReactionType, [FromQuery]int page, [FromQuery]int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var commentReactions =
            await _commentReactionService.GetCommentsReactions(commentId, commentReactionType, page, size);

        return Ok(commentReactions);
    }

    [HttpGet(ApiRoutes.CommentReaction.GetSpecificCommentReactionsCount)]
    public async Task<IActionResult> GetSpecificCommentReactionCount([FromQuery]Guid commentId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var specificCommentReactionsCount =
            await _commentReactionService.GetSpecificCommentReactionCount(commentId);

        return Ok(specificCommentReactionsCount);
    }

    [HttpDelete(ApiRoutes.CommentReaction.RemoveCommentReaction)]
    public async Task<IActionResult> RemoveCommentReaction([FromRoute]Guid commentReactionId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _commentReactionService.RemoveCommentReaction(commentReactionId);

        return Ok();
    }
}