using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Models.Series;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace GymAppInfrastructure.Services;

internal static class UtilsServices
{
    public static async Task<bool> CheckResourceAccessPermissions(Guid userIdFromJwt, Guid userIdFromResource, IUserRepo userRepo)
    {
        if (userIdFromJwt == userIdFromResource)
            return true;

        var friendStatus = await userRepo.GetFriend(userIdFromJwt, userIdFromResource);
        var user = await userRepo.GetOnlyValid(userIdFromResource);

        if (user is { ExtendedUser.PrivateAccount: true })
        {
            if (friendStatus is not null)
            {
                return friendStatus.FriendStatus == FriendStatus.Friend;
            }

            return false;
        }

        if (friendStatus is null)
            return true;
        
        return friendStatus.FriendStatus != FriendStatus.Blocked;
    }
}