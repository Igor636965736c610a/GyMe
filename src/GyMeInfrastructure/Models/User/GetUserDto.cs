namespace GymAppInfrastructure.Models.User;

public class GetUserDto
{
    public Guid Id { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public bool PrivateAccount { get; set; }
    public GenderDto Gender { get; set; }
    public string ProfilePictureUrl { get; set; }
    public string? Description { get; set; }
    public FriendStatusDto? FriendStatus { get; set; } 
}