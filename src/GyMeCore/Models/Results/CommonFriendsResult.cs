using GymAppCore.Models.Entities;
using GymAppCore.Models.Entities.Configurations;

namespace GymAppCore.Models.Results;

public class CommonFriendsResult
{
    public User User { get; set; }
    public int CommonFriendsCount { get; set; }
}