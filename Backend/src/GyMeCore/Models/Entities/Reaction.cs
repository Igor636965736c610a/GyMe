namespace GyMeCore.Models.Entities;

public class Reaction
{
    protected Reaction()
    {
    }

    public Reaction(Guid id, string? emoji, string? imageUrl, string reactionType, Guid simpleExerciseId, Guid userId)
    {
        Id = id;
        Emoji = emoji;
        ImageUrl = imageUrl;
        ReactionType = reactionType;
        SimpleExerciseId = simpleExerciseId;
        UserId = userId;
    }
    public Guid Id { get; set; }
    public string? Emoji { get; set; }
    public string? ImageUrl { get; set; }
    public string ReactionType { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    public SimpleExercise SimpleExercise { get; set; }
    public Guid SimpleExerciseId { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
}