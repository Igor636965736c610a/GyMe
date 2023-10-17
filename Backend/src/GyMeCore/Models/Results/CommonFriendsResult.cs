using GyMeCore.Models.Entities.Configurations;
using GyMeCore.Models.Entities;

namespace GyMeCore.Models.Results;

public class CommonFriendsResult
{
    public User User { get; set; }
    public UserFriend? UserFriend { get; set; }
    public int CommonFriendsCount { get; set; }
}