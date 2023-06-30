using System.ComponentModel.DataAnnotations;

namespace GymAppInfrastructure.ResetPasswordModel;

public class ResetPassword
{
    public string Password { get; set; }
    
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    public string Email { get; set; }
    public string Token { get; set; }
}