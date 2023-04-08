using System.Security.Claims;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.User;

namespace GymAppInfrastructure.IServices;

public interface IUserService
{
    Task<User?> Get(ClaimsPrincipal claimsPrincipal);
    Task Update(User user, PutUserDto putUserDto);
}