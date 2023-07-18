using GymAppInfrastructure.Dtos.Exercise;
using GymAppInfrastructure.Options;

public class GetUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public bool PrivateAccount { get; set; }
    public bool Valid { get; set; }
    public FriendStatus? FriendStatus { get; set; } = null;
    public Guid Id { get; set; } 
}