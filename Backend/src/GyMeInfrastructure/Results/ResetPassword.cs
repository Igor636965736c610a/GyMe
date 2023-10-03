using System.ComponentModel.DataAnnotations;

namespace GyMeInfrastructure.Results;

public class ResetPassword
{
    public string Password { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}