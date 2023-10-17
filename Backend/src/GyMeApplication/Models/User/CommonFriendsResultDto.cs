namespace GyMeApplication.Models.User;

public class CommonFriendsResultDto
{
    public GetUserDto User { get; set; }
    public string? FriendStatus { get; set; }
    public int CommonFriendsCount { get; set; }
}