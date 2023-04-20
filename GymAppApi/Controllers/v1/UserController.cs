using GymAppApi.BodyRequest.User;
using GymAppApi.Routes.v1;
using GymAppInfrastructure.IServices;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> AddFriend([FromBody]PostAddFriend postAddFriend)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var userToAddId = Guid.Parse(postAddFriend.UesrToAddId);

        await _userService.AddFriend(userId, userToAddId);

        return Ok();
    }

    [HttpDelete(ApiRoutes.User.DeleteFriendRequest)]
    public async Task<IActionResult> DeleteFriendRequest([FromBody] DeleteFriendRequest deleteFriendRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var friendRequestToDeleteId = Guid.Parse(deleteFriendRequest.UserToDelete);

        await _userService.RemoveFriendRequest(userId, friendRequestToDeleteId);

        return Ok();
    }
    
    [HttpDelete(ApiRoutes.User.DeleteFriend)]
    public async Task<IActionResult> DeleteFriend([FromBody] DeleteFriend deleteFriend)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var friendRequestToDeleteId = Guid.Parse(deleteFriend.UserToDelete);

        await _userService.RemoveFriend(userId, friendRequestToDeleteId);

        return Ok();
    }

    [HttpGet(ApiRoutes.User.GetUser)]
    public async Task<IActionResult> GetUser([FromQuery] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var userId = Guid.Parse(id);

        var result = await _userService.GetUser(userId);

        return Ok(result);
    }

    [HttpGet(ApiRoutes.User.ShowProfile)]
    public async Task<IActionResult> ShowProfile([FromQuery] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var userId = Guid.Parse(UtilsControllers.GetUserIdFromClaim(HttpContext));
        var profileId = Guid.Parse(id);

        var result =  await _userService.ShowProfile(userId, profileId);

        return Ok(result);
    }
}