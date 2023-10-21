using System.Net;
using GyMeApplication.IServices;
using GyMeApplication.Models.ReactionsAndComments.BodyRequest;
using GyMeWebApi.Routes.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GyMeWebApi.Controllers.v1;

[Authorize(Policy = "AppSys")]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    
    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost(ApiRoutes.Comments.AddComment)]
    public async Task<IActionResult> AddComment([FromBody]PostCommentDto postCommentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var commentId = await _commentService.AddComment(postCommentDto);
        
        var location = Url.Action("GetComment",new { id = commentId })!;

        return Created(location, commentId.ToString());
    }
    
    [HttpGet(ApiRoutes.Comments.GetComment)]
    public async Task<IActionResult> GetComment([FromRoute]string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var commentDto = await _commentService.GetComment(Guid.Parse(id));

        return Ok(commentDto);
    }

    [HttpGet(ApiRoutes.Comments.GetCommentsSortedByPubTime)]
    public async Task<IActionResult> GetCommentsSortedByPubTime([FromQuery]Guid simpleExerciseId, [FromQuery]int page, [FromQuery]int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comments = await _commentService.GetCommentsSortedByPubTime(simpleExerciseId, page, size);

        return Ok(comments);
    }
    
    [HttpGet(ApiRoutes.Comments.GetCommentsSortedByReactionsCount)]
    public async Task<IActionResult> GetCommentsSortedByReactionsCount([FromQuery]Guid simpleExerciseId, [FromQuery]int page, [FromQuery]int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var comments = await _commentService.GetCommentsSortedByReactionsCount(simpleExerciseId, page, size);

        return Ok(comments);
    }

    [HttpPut(ApiRoutes.Comments.UpdateComment)]
    public async Task<IActionResult> UpdateComment([FromRoute]Guid commentId, [FromBody]PutCommentDto putCommentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = await _commentService.UpdateComment(putCommentDto, commentId);

        return Ok(comment);
    }

    [HttpDelete(ApiRoutes.Comments.RemoveComment)]
    public async Task<IActionResult> RemoveComment([FromRoute]string commentId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _commentService.RemoveComment(Guid.Parse(commentId));

        return Ok();
    }
}