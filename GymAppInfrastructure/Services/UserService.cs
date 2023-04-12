using System.Security.Claims;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.User;
using GymAppInfrastructure.IServices;
using Microsoft.AspNetCore.Identity;

namespace GymAppInfrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IUserRepo _userRepo;
    public UserService(UserManager<User> userManager, IUserRepo userRepo)
    {
        _userManager = userManager;
        _userRepo = userRepo;
    }
    
    

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