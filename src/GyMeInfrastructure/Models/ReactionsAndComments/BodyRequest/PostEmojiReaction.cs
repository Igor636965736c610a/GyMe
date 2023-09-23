namespace GymAppInfrastructure.Models.ReactionsAndComments.BodyRequest.BodyRequest;

public class PostEmojiReaction
{
    public Guid SimpleExerciseId { get; set; }
    public ReactionType ReactionType { get; set; }
}