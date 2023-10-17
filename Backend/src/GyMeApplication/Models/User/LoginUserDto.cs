using System.ComponentModel.DataAnnotations;

namespace GyMeApplication.Models.User;

public class LoginUserDto
{
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}