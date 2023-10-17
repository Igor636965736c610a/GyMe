namespace GyMeApplication.Models.ReactionsAndComments.BodyRequest.BodyRequest;

public class PostReactionDto
{
    public Guid SimpleExerciseId { get; set; }
    public ReactionType ReactionType { get; set; }
}