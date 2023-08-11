using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace GymAppCore.Models.Entities;

public class User : IdentityUser<Guid>
{
    public override Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public override string? UserName { get; set; }
    public Gender Gender { get; set; }
    [Column(TypeName = "bytea")]
    public byte[] ProfilePicture { get; set; }
    public bool PrivateAccount { get; set; }
    public bool Valid { get; set; }
    public string AccountProvider { get; set; }
    public List<UserFriend> Friends { get; set; }
    public List<UserFriend> InverseFriends { get; set; }
    public List<Exercise> Exercises { get; set; } 
    public List<SimpleExercise> SimpleExercises { get; set; }
    public bool Premium { get; set; } = false;
    public DateTime? ImportancePremium { get; set; }
}