using System.ComponentModel.DataAnnotations.Schema;

namespace GymAppCore.Models.Entities;

public class ExtendedUser
{
    public Gender Gender { get; set; }
    [Column(TypeName = "bytea")]
    public byte[] ProfilePicture { get; set; }
    public bool PrivateAccount { get; set; }
    public bool Premium { get; set; } 
    public DateTime? ImportancePremium { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
}