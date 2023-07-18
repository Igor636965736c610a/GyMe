using GymAppCore.Models.Entities;

namespace GymAppCore.IRepo;

public interface IUserRepo
{
    Task<bool> RemoveFriendRequest(FriendRequest friendRequest);
    Task<bool> AddFriendRequest(FriendRequest friendRequest);
    Task<FriendRequest?> GetFriendRequest(Guid senderId, Guid recipientId);
    Task<User?> ShowProfile(Guid id);
    Task<User?> Get(Guid id);
    Task<User?> Get(string userName);
    Task<List<User>> FindUsers(string key, int page, int size);
    Task<List<User>> GetFriends(Guid id, int page, int size);
    Task<UserFriend?> GetFriend(Guid user1Id, Guid user2Id);
    Task<bool> AddFriend(List<UserFriend> userFriend);
    Task<bool> RemoveFriend(UserFriend userFriend);
    Task<bool> RemoveFriend(List<UserFriend> userFriend);
    Task<bool> Update(User user);
    Task<bool> RemoveUser(User user);
}