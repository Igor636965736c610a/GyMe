using GyMeApplication.IServices;
using GyMeApplication.Models.User;
using GyMeWebApi.Routes.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GyMeWebApi.Controllers.v1;

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
    public async Task<IActionResult> AddFriend([FromRoute] Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _userService.AddFriend(id);

        return Ok();
    }

    [HttpDelete(ApiRoutes.User.DeleteFriend)]
    public async Task<IActionResult> DeleteFriend([FromRoute] Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _userService.RemoveFriend(id);

        return Ok();
    }

    [HttpGet(ApiRoutes.User.GetUser)]
    public async Task<IActionResult> GetUser([FromRoute] Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.GetUser(id);

        return Ok(result);
    }

    [HttpGet(ApiRoutes.User.GetFriends)]
    public async Task<IActionResult> GetFriends([FromQuery] FriendStatusDto friendStatusDto, [FromQuery] int page,
        [FromQuery] int size)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var friends = await _userService.GetFriends(friendStatusDto, page, size);

        return Ok(friends);
    }

    [HttpGet(ApiRoutes.User.GetCommonFriends)]
    public async Task<IActionResult> GetCommonFriends([FromQuery] int page, [FromQuery] int size)
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

    [HttpPost(ApiRoutes.User.BlockUser)]
    public async Task<IActionResult> BlockUser([FromQuery] Guid userToBlockId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _userService.BlockUser(userToBlockId);

        return Ok();
    }

    [HttpPost(ApiRoutes.User.UnblockUser)]
    public async Task<IActionResult> UnblockUser([FromQuery] Guid userToUnblockBlockId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _userService.UnblockUser(userToUnblockBlockId);

        return Ok();
    }
}