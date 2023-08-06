using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Series;

namespace GymAppInfrastructure.Services;

internal static class UtilsServices
{
    internal static async Task<bool> CheckResourceAccessPermissions(Guid userIdFromJwt, Guid userIdFromResource, IUserRepo userRepo)
    {
        if (userIdFromJwt == userIdFromResource)
            return true;
        
        var user = await userRepo.Get(userIdFromResource);
        if (!user!.PrivateAccount)
            return true;

        var friendStatus = await userRepo.GetFriend(userIdFromJwt, userIdFromResource);
        
        return friendStatus is not null && friendStatus.FriendStatus == FriendStatus.Friend;
    }
}