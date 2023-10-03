using System.ComponentModel.DataAnnotations;

namespace GymAppInfrastructure.Models.User;

public class LoginUserDto
{
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}