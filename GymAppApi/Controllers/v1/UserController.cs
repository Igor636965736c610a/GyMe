using GymAppApi.BodyRequest.User;
using GymAppApi.Routes.v1;
using GymAppInfrastructure.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GymAppApi.Controllers.v1;

[Authorize(Policy = "SSO")]
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
        var jwtId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var userToAddId = Guid.Parse(id);

        await _userService.AddFriend(jwtId, userToAddId);

        return Ok();
    }

    [HttpDelete(ApiRoutes.User.DeleteFriendRequest)]
    public async Task<IActionResult> DeleteFriendRequest([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var jwtId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var friendRequestToDeleteId = Guid.Parse(id);

        await _userService.RemoveFriendRequest(jwtId, friendRequestToDeleteId);

        return Ok();
    }
    
    [HttpDelete(ApiRoutes.User.DeleteFriend)]
    public async Task<IActionResult> DeleteFriend([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var jwtId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var friendRequestToDeleteId = Guid.Parse(id);

        await _userService.RemoveFriend(jwtId, friendRequestToDeleteId);

        return Ok();
    }

    [HttpGet(ApiRoutes.User.GetUser)]
    public async Task<IActionResult> GetUser([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var jwtId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var userId = Guid.Parse(id);

        var result = await _userService.GetUser(jwtId, userId);

        return Ok(result);
    }

    [HttpGet(ApiRoutes.User.GetFriends)]
    public async Task<IActionResult> GetFriends([FromQuery]int page, [FromQuery]int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var jwtId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));

        var friends = await _userService.GetFriends(jwtId, page, size);

        return Ok(friends);
    }
}