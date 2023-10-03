using GymAppApi.Routes.v1;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GymAppApi.Controllers.v1;

[Authorize(Policy = "AppSys")]
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
        
        var userToAddId = Guid.Parse(id);

        await _userService.AddFriend(userToAddId);

        return Ok();
    }
    
    [HttpDelete(ApiRoutes.User.DeleteFriend)]
    public async Task<IActionResult> DeleteFriend([FromRoute] string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var friendRequestToDeleteId = Guid.Parse(id);

        await _userService.RemoveFriend(friendRequestToDeleteId);

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
    public async Task<IActionResult> GetFriends([FromQuery]FriendStatusDto friendStatusDto, [FromQuery]int page, [FromQuery]int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var friends = await _userService.GetFriends(friendStatusDto, page, size);

        return Ok(friends);
    }

    [HttpGet(ApiRoutes.User.GetCommonFriends)]
    public async Task<IActionResult> GetCommonFriends([FromQuery]int page, [FromQuery]int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var commonFriends = await _userService.GetCommonFriends(page, size);

        return Ok(commonFriends);
    }

    [HttpGet(ApiRoutes.User.FindUser)]
    public async Task<IActionResult> FindUser([FromQuery] string key, [FromQuery] int page, [FromQuery] int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var users = await _userService.FindUsers(key, page, size);

        return Ok(users);
    }
}