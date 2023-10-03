using System.Security.Claims;
using GyMeCore.Models.Entities;
using GyMeInfrastructure.Models.User;

namespace GyMeInfrastructure.IServices;

public interface IUserService
{
    Task RemoveFriend(Guid userToRemoveId);
    Task AddFriend(Guid userToAddId);
    Task<IEnumerable<GetUserDto>> FindUsers(string key, int page, int size);
    Task<IEnumerable<CommonFriendsResultDto>> GetCommonFriends(int page, int size);
    Task BlockUser(Guid userId);
    Task UnblockUser(Guid userId);
    Task<GetUserDto> GetUser(Guid id);
    Task<IEnumerable<GetUserDto>> GetFriends(FriendStatusDto friendStatusDto, int page, int size);
}