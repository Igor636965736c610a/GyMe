using GymAppCore.Models.Entities;
using GymAppCore.Models.Results;

namespace GymAppCore.IRepo;

public interface IUserRepo
{
    Task<User?> GetOnlyValid(Guid id);
    Task<User?> Get(Guid id);
    Task<User?> Get(string userName);
    Task<List<User>> FindUsers(string key, int page, int size);
    Task<List<UserFriend>> GetFriends(Guid id, FriendStatus friendStatus, int page, int size);
    Task<UserFriend?> GetFriend(Guid userId, Guid friendId);
    Task<IEnumerable<CommonFriendsResult>> GetCommonFriendsSortedByCount(Guid userId, int page, int size = 50);
    Task<bool> AddFriend(IEnumerable<UserFriend> userFriend);
    Task<bool> RemoveFriend(UserFriend userFriend);
    Task<bool> RemoveFriend(IEnumerable<UserFriend> userFriend);
    Task<bool> Update(User user);
    Task<bool> RemoveUser(User user);
    Task<ResourcesAddresses?> GetResourcesAddresses(Guid userId);
    Task AddResourcesAddresses(ResourcesAddresses resourcesAddresses);
}