namespace GymAppInfrastructure.Models.User;

public class PutUserDto
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsChlopak { get; set; }
    public byte[] ProfilePicture { get; set; }
    public bool PrivateAccount { get; set; }
}