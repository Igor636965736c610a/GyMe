using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GymAppInfrastructure.Repo;

internal class UserRepo : IUserRepo
{
    private readonly GymAppContext _gymAppContext;
    public UserRepo(GymAppContext gymAppContext)
    {
        _gymAppContext = gymAppContext;
    }

    public async Task<bool> RemoveFriendRequest(FriendRequest friendRequest)
    {
        _gymAppContext.Remove(friendRequest);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> AddFriendRequest(FriendRequest friendRequest)
    {
        await _gymAppContext.FriendRequests.AddAsync(friendRequest);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<FriendRequest?> GetFriendRequest(Guid senderId, Guid recipientId)
        => await _gymAppContext.FriendRequests.FirstOrDefaultAsync(x =>
            x.SenderId == senderId && x.RecipientId == recipientId);

    public async Task<User?> ShowProfile(Guid id)
        => await _gymAppContext.Users.Include(x => x.Exercises).FirstOrDefaultAsync(x => x.Id == id);
    
    public async Task<User?> Get(Guid id)
        => await _gymAppContext.Users.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<User?> Get(string userName)
        => await _gymAppContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);

    public async Task<List<User>> FindUsers(string key, int page, int size)
        => await _gymAppContext.Users.Where(x => $"{x.FirstName}{x.LastName}".Contains(key) || x.UserName.Contains(key))
            .Skip(page*size)
            .Take(size).ToListAsync();

    public async Task<List<User>> GetFriends(Guid id, int page, int size)
        => await _gymAppContext.UserFriends.Where(x => x.UserId == id).Select(x => x.Friend)
            .Skip(page*size)
            .Take(size).ToListAsync();

    public async Task<UserFriend?> GetFriend(Guid user1Id, Guid user2Id)
        => await _gymAppContext.UserFriends.FirstOrDefaultAsync(x => x.UserId == user1Id && x.FriendId == user2Id);

    public async Task<bool> AddFriend(List<UserFriend> userFriend)
    {
        await _gymAppContext.UserFriends.AddRangeAsync(userFriend);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> RemoveFriend(UserFriend userFriend)
    {
        _gymAppContext.UserFriends.Remove(userFriend);
        return await UtilsRepo.Save(_gymAppContext);
    }
    
    public async Task<bool> RemoveFriend(List<UserFriend> userFriend)
    {
        _gymAppContext.UserFriends.RemoveRange(userFriend);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> Update(User user)
    {
        _gymAppContext.Users.Update(user);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> RemoveUser(User user)
    {
        _gymAppContext.Users.Remove(user);
        return await UtilsRepo.Save(_gymAppContext);
    }
}