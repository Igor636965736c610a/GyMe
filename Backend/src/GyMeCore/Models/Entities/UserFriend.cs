namespace GymAppCore.Models.Entities;

public class UserFriend
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid FriendId { get; set; }
    public User Friend { get; set; }
    public FriendStatus FriendStatus { get; set; }
}