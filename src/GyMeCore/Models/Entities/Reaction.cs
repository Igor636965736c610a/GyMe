namespace GymAppCore.Models.Entities;

public class Reaction
{
    protected Reaction()
    {
    }

    public Reaction(Guid id, string? emoji, string imageUel, string reactionType, Guid simpleExerciseId, Guid userId)
    {
        Id = id;
        Emoji = emoji;
        ImageUel = imageUel;
        ReactionType = reactionType;
        SimpleExerciseId = simpleExerciseId;
        UserId = userId;
    }
    public Guid Id { get; set; }
    public string? Emoji { get; set; }
    public string ImageUel { get; set; }
    public string ReactionType { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    public SimpleExercise SimpleExercise { get; set; }
    public Guid SimpleExerciseId { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}