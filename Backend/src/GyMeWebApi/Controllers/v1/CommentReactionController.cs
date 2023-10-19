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

        return Ok(commentReactionId.ToString());
    }

    [HttpGet(ApiRoutes.CommentReaction.GetCommentReactions)]
    public async Task<IActionResult> GetCommentReactions([FromQuery]string commentId, [FromQuery]CommentReactionType? commentReactionType, [FromQuery]int page, [FromQuery]int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var commentReactions =
            await _commentReactionService.GetCommentsReactions(Guid.Parse(commentId), commentReactionType, page, size);

        return Ok(commentReactions);
    }

    [HttpGet(ApiRoutes.CommentReaction.GetSpecificCommentReactionsCount)]
    public async Task<IActionResult> GetSpecificCommentReactionCount([FromQuery] string commentId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var specificCommentReactionsCount =
            await _commentReactionService.GetSpecificCommentReactionCount(Guid.Parse(commentId));

        return Ok(specificCommentReactionsCount);
    }

    [HttpDelete(ApiRoutes.CommentReaction.RemoveCommentReaction)]
    public async Task<IActionResult> RemoveCommentReaction([FromRoute]string commentReactionId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _commentReactionService.RemoveCommentReaction(Guid.Parse(commentReactionId));

        return Ok();
    }
}