using GymAppInfrastructure.Dtos.Exercise;

public class GetUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<GetUserDto> Friends { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<GetExerciseDto> ExercisesDto { get; set; }
}