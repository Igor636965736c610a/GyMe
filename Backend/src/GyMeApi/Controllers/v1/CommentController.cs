using GyMeInfrastructure.IServices;
using GyMeInfrastructure.Models.ReactionsAndComments.BodyRequest;
using GyMeApi.Routes.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GyMeApi.Controllers.v1;

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

        await _commentService.AddComment(postCommentDto);

        return Ok();
    }
    
    [HttpGet(ApiRoutes.Comments.GetComment)]
    public async Task<IActionResult> GetComment([FromRoute]string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _commentService.GetComment(Guid.Parse(id));

        return Ok();
    }

    [HttpGet(ApiRoutes.Comments.GetCommentsSortedByPubTime)]
    public async Task<IActionResult> GetCommentsSortedByPubTime([FromQuery]Guid simpleExerciseId, [FromQuery]int page, [FromQuery]int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _commentService.GetCommentsSortedByPubTime(simpleExerciseId, page, size);

        return Ok();
    }
    
    [HttpGet(ApiRoutes.Comments.GetCommentsSortedByReactionsCount)]
    public async Task<IActionResult> GetCommentsSortedByReactionsCount([FromQuery]Guid simpleExerciseId, [FromQuery]int page, [FromQuery]int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _commentService.GetCommentsSortedByReactionsCount(simpleExerciseId, page, size);

        return Ok();
    }

    [HttpPut(ApiRoutes.Comments.UpdateComment)]
    public async Task<IActionResult> UpdateComment([FromRoute] string commentId, [FromBody] PutCommentDto putCommentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _commentService.UpdateComment(putCommentDto, Guid.Parse(commentId));

        return Ok();
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