namespace GyMeApplication.Models.ReactionsAndComments.BodyRequest;

public class PostCommentDto
{
    public Guid SimpleExerciseId { get; set; }
    public string Message { get; set; }
}