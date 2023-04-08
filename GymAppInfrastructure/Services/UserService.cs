using System.Security.Claims;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.User;
using GymAppInfrastructure.IServices;
using Microsoft.AspNetCore.Identity;

namespace GymAppInfrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User?> Get(ClaimsPrincipal claimsPrincipal)
        => await _userManager.GetUserAsync(claimsPrincipal);

    public async Task Update(User user, PutUserDto putUserDto)
    {
        user.UserName = putUserDto.UserName;
        user.FirstName = putUserDto.FirstName;
        user.LastName = putUserDto.LastName;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException("Something went wrong");
    }
}