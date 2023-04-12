using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GymAppInfrastructure.Repo;

public class UserRepo : IUserRepo
{
    private readonly GymAppContext _gymAppContext;
    public UserRepo(GymAppContext gymAppContext)
    {
        _gymAppContext = gymAppContext;
    }

    public async Task<User?> Get(Guid id)
        => await _gymAppContext.Users.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<User>> GetFriends(Guid id)
        => await _gymAppContext.UserFriends.Where(x => x.UserId == id).Select(x => x.Friend).ToListAsync();

    public async Task<bool> AddFriend(UserFriend userFriend)
    {
        await _gymAppContext.UserFriends.AddAsync(userFriend);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> RemoveFriend(UserFriend userFriend)
    {
        _gymAppContext.UserFriends.Remove(userFriend);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> Update(User user)
    {
        _gymAppContext.Users.Update(user);
        return await UtilsRepo.Save(_gymAppContext);
    }
}