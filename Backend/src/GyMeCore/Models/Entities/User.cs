using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace GymAppCore.Models.Entities;

public class User : IdentityUser<Guid>
{
    public override Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public override string UserName { get; set; }
    public override string Email { get; set; }
    public bool Valid { get; set; }
    public string AccountProvider { get; set; }
    public DateTime LastRefreshMainPage { get; set; } = DateTime.UtcNow;
    public ResourcesAddresses SetResourcesAddresses { get; set; }
    public ExtendedUser? ExtendedUser { get; set; }
    public List<UserFriend> Friends { get; set; }
    public List<UserFriend> InverseFriends { get; set; }
    public List<Exercise> Exercises { get; set; } 
    public List<SimpleExercise> SimpleExercises { get; set; }
    public List<Reaction> Reactions { get; set; }
    public List<Comment> Comments { get; set; }
    public List<CommentReaction> CommentReactions { get; set; }
}