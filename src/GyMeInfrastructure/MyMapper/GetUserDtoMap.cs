using GymAppCore.Models.Entities;
using GymAppInfrastructure.Models.User;

namespace GymAppInfrastructure.MyMapper;

public static class GetUserDtoMap
{
    public static GetUserDto Map(User user, FriendStatus? friendStatus = null)
        => new GetUserDto()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            PrivateAccount = user.ExtendedUser.PrivateAccount,
            Gender = (GenderDto)user.ExtendedUser.Gender,
            ProfilePicture = user.ExtendedUser.ProfilePicture,
            Description = user.ExtendedUser.Description,
            FriendStatus = friendStatus.HasValue ? (FriendStatusDto)friendStatus : null
        };

    public static IEnumerable<GetUserDto> Map(IEnumerable<UserFriend> userFriends)
        => userFriends.Select(x => Map(x.Friend, x.FriendStatus));
}