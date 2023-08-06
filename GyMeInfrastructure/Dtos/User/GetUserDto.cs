using GymAppInfrastructure.Dtos.User;

public class GetUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public bool PrivateAccount { get; set; }
    public FriendStatusDto? FriendStatus { get; set; } = null;
    public Guid Id { get; set; } 
}