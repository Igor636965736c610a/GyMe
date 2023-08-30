using System.Security.Claims;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Models.User;

namespace GymAppInfrastructure.IServices;

public interface IUserService
{
    Task RemoveFriend(Guid userToRemoveId);
    Task AddFriend(Guid userToAddId);
    Task<IEnumerable<Guid>> FindUsers(string key, int page, int size);
    Task<IEnumerable<CommonFriendsResultDto>> GetCommonFriends(int page, int size);
    Task<GetUserDto> GetUser(Guid id);
    Task<IEnumerable<GetUserDto>> GetFriends(FriendStatusDto friendStatusDto, int page, int size);
}