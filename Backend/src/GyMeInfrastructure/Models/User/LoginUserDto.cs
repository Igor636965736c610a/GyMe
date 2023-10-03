using System.ComponentModel.DataAnnotations;

namespace GyMeInfrastructure.Models.User;

public class LoginUserDto
{
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}