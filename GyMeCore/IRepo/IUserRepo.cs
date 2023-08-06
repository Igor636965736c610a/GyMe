using GymAppCore.Models.Entities;

namespace GymAppCore.IRepo;

public interface IUserRepo
{
    Task<User?> ShowProfile(Guid id);
    Task<User?> Get(Guid id);
    Task<User?> Get(string userName);
    Task<List<User>> FindUsers(string key, int page, int size);
    Task<List<User>> GetFriends(Guid id, int page, int size);
    Task<UserFriend?> GetFriend(Guid user, Guid friend);
    Task<bool> AddFriend(IEnumerable<UserFriend> userFriend);
    Task<bool> RemoveFriend(UserFriend userFriend);
    Task<bool> RemoveFriend(IEnumerable<UserFriend> userFriend);
    Task<bool> Update(User user);
    Task<bool> RemoveUser(User user);
}