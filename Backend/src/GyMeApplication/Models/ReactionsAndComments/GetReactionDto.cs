namespace GyMeApplication.Models.ReactionsAndComments;

public class GetReactionDto
{
    public Guid Id { get; set; }
    public string? Emoji { get; set; }
    public string? ImageReaction { get; set; }
    public Owner User { get; set; }
    public Guid SimpleExerciseId { get; set; }
    public string ReactionTypeDto { get; set; }
    public DateTime TimeStamp { get; set; }
}