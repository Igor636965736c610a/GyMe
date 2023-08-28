namespace GymAppInfrastructure.Models.User;

public class CommonFriendsResultDto
{
    public GetUserDto User { get; set; }
    public int CommonFriendsCount { get; set; }
}