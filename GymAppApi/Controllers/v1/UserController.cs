using GymAppApi.BodyRequest.User;
using GymAppApi.Routes.v1;
using GymAppInfrastructure.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GymAppApi.Controllers.v1;

[Authorize]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost(ApiRoutes.User.AddFriend)]
    public async Task<IActionResult> AddFriend([FromRoute]string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var userToAddId = Guid.Parse(id);

        await _userService.AddFriend(userId, userToAddId);

        return Ok();
    }

    [HttpDelete(ApiRoutes.User.DeleteFriendRequest)]
    public async Task<IActionResult> DeleteFriendRequest([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var friendRequestToDeleteId = Guid.Parse(id);

        await _userService.RemoveFriendRequest(userId, friendRequestToDeleteId);

        return Ok();
    }
    
    [HttpDelete(ApiRoutes.User.DeleteFriend)]
    public async Task<IActionResult> DeleteFriend([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var friendRequestToDeleteId = Guid.Parse(id);

        await _userService.RemoveFriend(userId, friendRequestToDeleteId);

        return Ok();
    }

    [HttpGet(ApiRoutes.User.GetUser)]
    public async Task<IActionResult> GetUser([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var userId = Guid.Parse(id);

        var result = await _userService.GetUser(userId);

        return Ok(result);
    }

    [HttpGet(ApiRoutes.User.GetFriends)]
    public async Task<IActionResult> GetFriends([FromQuery]int page, [FromQuery]int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));

        var friends = await _userService.GetFriends(userId, page, size);

        return Ok(friends);
    }
}