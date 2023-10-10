using GyMeCore.IRepo;
using GyMeCore.Models.Entities;
using GyMeCore.Models.Results;
using GyMeInfrastructure.Options;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;

namespace GyMeInfrastructure.Repo;

internal class UserRepo : IUserRepo
{
    private readonly GyMePostgresContext _gyMePostgresContext;
    public UserRepo(GyMePostgresContext gyMePostgresContext)
    {
        _gyMePostgresContext = gyMePostgresContext;
    }
    
    public async Task<User?> Get(Guid id)
        => await _gyMePostgresContext.Users.Include(x => x.ExtendedUser).FirstOrDefaultAsync(x => x.Id == id);
    
    public async Task<User?> GetOnlyValid(Guid id)
        => await _gyMePostgresContext.Users.Include(x => x.ExtendedUser).FirstOrDefaultAsync(x => x.Id == id && x.Valid);

    public async Task<User?> Get(string userName)
        => await _gyMePostgresContext.Users.Include(x => x.ExtendedUser).FirstOrDefaultAsync(x => x.UserName == userName);

    public async Task<List<User>> FindUsers(string key, int page, int size)
        => await _gyMePostgresContext.Users.Where(x => (x.FirstName + x.LastName).ToLower().Contains(key.ToLower().Trim()) || x.UserName.Contains(key) && x.Valid)
            .Skip(page*size)
            .Take(size)
            .Include(x => x.ExtendedUser)
            .ToListAsync();

    public async Task<List<UserFriend>> GetFriends(Guid id, FriendStatus friendStatus, int page, int size)
        => await _gyMePostgresContext.UserFriends.Where(x => x.UserId == id && x.FriendStatus == friendStatus)
            .Include(x => x.Friend)
            .ThenInclude(x => x.ExtendedUser)
            .Skip(page*size)
            .Take(size)
            .ToListAsync();

    public async Task<UserFriend?> GetFriend(Guid userId, Guid friendId)
        => await _gyMePostgresContext.UserFriends.FirstOrDefaultAsync(x => x.UserId == userId && x.FriendId == friendId);

    public async Task<IEnumerable<CommonFriendsResult>> GetCommonFriendsSortedByCount(Guid userId, int page, int size = 20)
        => await _gyMePostgresContext.UserFriends
            .Where(x => x.UserId == userId)
            .Include(x => x.Friend.Friends)
            .SelectMany(x => x.Friend.Friends)
            .Where(x => x.FriendId != userId && x.FriendStatus == FriendStatus.Friend)
            .GroupBy(x => x.Friend)
            .OrderBy(x => x.Count())
            .Skip(page*size)
            .Take(size)
            .Include(x => x.Key.ExtendedUser)
            .Select(group => new CommonFriendsResult
            {
                User = group.Key,
                CommonFriendsCount = group.Count()
            })
            .ToListAsync();

    public async Task AddFriend(IEnumerable<UserFriend> userFriend)
    {
        await _gyMePostgresContext.UserFriends.AddRangeAsync(userFriend);
        await _gyMePostgresContext.SaveChangesAsync();
    }

    public async Task RemoveFriend(UserFriend userFriend)
    {
        _gyMePostgresContext.UserFriends.Remove(userFriend);
        await _gyMePostgresContext.SaveChangesAsync();
    }
    
    public async Task RemoveFriend(IEnumerable<UserFriend> userFriend)
    {
        _gyMePostgresContext.UserFriends.RemoveRange(userFriend);
        await _gyMePostgresContext.SaveChangesAsync();
    }

    public async Task Update(User user)
    {
        _gyMePostgresContext.Users.Update(user);
        await _gyMePostgresContext.SaveChangesAsync();
    }

    public async Task UpdateFriendsStatus(IEnumerable<UserFriend> userFriends)
    {
        _gyMePostgresContext.UserFriends.UpdateRange(userFriends);
        await _gyMePostgresContext.SaveChangesAsync();
    }

    public async Task RemoveUser(User user)
    {
        _gyMePostgresContext.Users.Remove(user);
        await _gyMePostgresContext.SaveChangesAsync();
    }

    public async Task<ResourcesAddresses?> GetResourcesAddresses(Guid userId)
        => await _gyMePostgresContext.ResourcesAddresses
            .FirstOrDefaultAsync(x => x.UserId == userId);

    public async Task AddResourcesAddresses(ResourcesAddresses resourcesAddresses)
    {
        await _gyMePostgresContext.ResourcesAddresses
            .AddAsync(resourcesAddresses);

        await _gyMePostgresContext.SaveChangesAsync();
    }
}