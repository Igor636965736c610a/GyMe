using GymAppInfrastructure.Dtos.Exercise;

namespace GymAppInfrastructure.Dtos.User;

public class ShowProfileDto
{
    public  Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public bool PrivateAccount { get; set; }
    public List<GetExerciseDto> Exercises { get; set; } 
}