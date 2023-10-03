namespace GyMeCore.Models.Entities;

public class Comment
{
    protected Comment()
    {
    }

    public Comment(Guid id, string message, Guid simpleExerciseId, Guid userId)
    {
        Id = id;
        Message = message;
        SimpleExerciseId = simpleExerciseId;
        UserId = userId;
    }
    public Guid Id { get; set; }
    public string Message { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    public SimpleExercise SimpleExercise { get; set; }
    public Guid SimpleExerciseId { get; set; }
    public List<CommentReaction> CommentReactions { get; set; }
    public virtual User User { get; set; }
    public Guid UserId { get; set; }
}