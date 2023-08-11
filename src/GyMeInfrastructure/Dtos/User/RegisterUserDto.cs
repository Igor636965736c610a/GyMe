using System.ComponentModel.DataAnnotations;

namespace GymAppInfrastructure.Dtos.User;

public class RegisterUserDto
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool PrivateAccount { get; set; }
    public bool IsChlopak { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}