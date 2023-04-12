using GymAppCore.Models.Entities;

namespace GymAppCore.IRepo;

public interface IUserRepo
{
    Task<User?> Get(Guid id);
    Task<List<User>> GetFriends(Guid id);
    Task<bool> AddFriend(UserFriend userFriend);
    Task<bool> RemoveFriend(UserFriend userFriend);
    Task<bool> Update(User user);
}