using System.ComponentModel.DataAnnotations.Schema;

namespace GyMeCore.Models.Entities;

public class ExtendedUser
{
    public Guid UserId { get; set; }
    public string Gender { get; set; }
    public string ProfilePictureUrl { get; set; }
    public bool PrivateAccount { get; set; }
    public string? Description { get; set; }
    public User User { get; set; }
}