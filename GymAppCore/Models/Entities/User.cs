using Microsoft.AspNetCore.Identity;

namespace GymAppCore.Models.Entities;

public class User : IdentityUser<Guid>
{
    public override Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool PrivateAccount { get; set; }
    public string AccountProvider { get; set; } = "App";
    public List<UserFriend> Friends { get; set; }
    public List<UserFriend> InverseFriends { get; set; }
    public List<FriendRequest> SendFriendRequests { get; set; }
    public List<FriendRequest> RecipientFriendRequests { get; set; }
    public List<Exercise> Exercises { get; set; } 
    public List<SimpleExercise> SimpleExercises { get; set; }
    public bool Premium { get; set; } = false;
    public DateTime? ImportancePremium { get; set; }
}