using Microsoft.AspNetCore.Identity;

namespace GymAppCore.Models.Entities;

public class User : IdentityUser<Guid>
{
    public override Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<User> Friends { get; set; } 
    public List<Exercise> Exercises { get; set; } 
    public List<SimpleExercise> SimpleExercises { get; set; }
    public Premium Premium { get; set; }
}