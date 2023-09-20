namespace GymAppInfrastructure.Models.ReactionsAndComments.BodyRequest;

public class PostEmojiReaction
{
    public Guid SimpleExerciseId { get; set; }
    public Emoji Emoji { get; set; }
}