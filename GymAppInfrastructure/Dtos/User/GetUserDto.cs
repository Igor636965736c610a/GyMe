using GymAppInfrastructure.Dtos.Exercise;

public class GetUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public bool PrivateAccount { get; set; }
    public Guid Id { get; set; } 
}