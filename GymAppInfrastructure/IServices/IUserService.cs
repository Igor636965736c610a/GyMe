using System.Security.Claims;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.User;

namespace GymAppInfrastructure.IServices;

public interface IUserService
{
    Task RemoveFriend(Guid user1Id, Guid user2Id);
    Task RemoveFriendRequest(Guid user1Id, Guid user2Id);
    Task AddFriend(Guid user1Id, Guid user2Id);
    Task<List<GetUserDto>> FindUsers(string key, int page, int size);
    Task<GetUserDto> GetUser(Guid id);
    Task<IEnumerable<GetUserDto>> GetFriends(Guid userId, int page, int size);
}