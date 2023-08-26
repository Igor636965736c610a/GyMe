using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GymAppInfrastructure.Models.User;

public class RegisterUserDto
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool PrivateAccount { get; set; }
    public bool IsChlopak { get; set; }
    public string? Description { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}