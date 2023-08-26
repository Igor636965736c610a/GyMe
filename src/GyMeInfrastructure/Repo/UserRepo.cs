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
    
    public async Task<User?> Get(Guid id)
        => await _gymAppContext.Users.Include(x => x.ExtendedUser).FirstOrDefaultAsync(x => x.Id == id);
    
    public async Task<User?> GetOnlyValid(Guid id)
        => await _gymAppContext.Users.Include(x => x.ExtendedUser).FirstOrDefaultAsync(x => x.Id == id && x.Valid);

    public async Task<User?> Get(string userName)
        => await _gymAppContext.Users.Include(x => x.ExtendedUser).FirstOrDefaultAsync(x => x.UserName == userName);

    public async Task<List<User>> FindUsers(string key, int page, int size)
        => await _gymAppContext.Users.Where(x => (x.FirstName + x.LastName).ToLower().Contains(key.ToLower().Trim()) || x.UserName.Contains(key) && x.Valid)
            .Skip(page*size)
            .Take(size)
            .Include(x => x.ExtendedUser)
            .ToListAsync();

    public async Task<List<User>> GetFriends(Guid id, int page, int size)
        => await _gymAppContext.UserFriends.Where(x => x.UserId == id && x.FriendStatus == FriendStatus.Friend).Select(x => x.Friend)
            .Skip(page*size)
            .Take(size)
            .ToListAsync();

    public async Task<UserFriend?> GetFriend(Guid user, Guid friend)
        => await _gymAppContext.UserFriends.FirstOrDefaultAsync(x => x.UserId == user);

    public async Task<bool> AddFriend(IEnumerable<UserFriend> userFriend)
    {
        await _gymAppContext.UserFriends.AddRangeAsync(userFriend);
        return await UtilsRepo.SaveDatabaseChanges(_gymAppContext);
    }

    public async Task<bool> RemoveFriend(UserFriend userFriend)
    {
        _gymAppContext.UserFriends.Remove(userFriend);
        return await UtilsRepo.SaveDatabaseChanges(_gymAppContext);
    }
    
    public async Task<bool> RemoveFriend(IEnumerable<UserFriend> userFriend)
    {
        _gymAppContext.UserFriends.RemoveRange(userFriend);
        return await UtilsRepo.SaveDatabaseChanges(_gymAppContext);
    }

    public async Task<bool> Update(User user)
    {
        _gymAppContext.Users.Update(user);
        return await UtilsRepo.SaveDatabaseChanges(_gymAppContext);
    }

    public async Task<bool> RemoveUser(User user)
    {
        _gymAppContext.Users.Remove(user);
        return await UtilsRepo.SaveDatabaseChanges(_gymAppContext);
    }
}