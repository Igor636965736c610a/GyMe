namespace GyMeCore.Models.Entities;

public class CommentReaction
{
    protected CommentReaction()
    {
    }

    public CommentReaction(Guid id, string emoji, Guid commentId, Guid userId)
    {
        Id = id;
        Emoji = emoji;
        CommentId = commentId;
        UserId = userId;
    }
    public Guid Id { get; set; }
    public string Emoji { get; set; }
    public string ReactionType { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    public Comment Comment { get; set; }
    public Guid CommentId { get; set; }
    public virtual User User { get; set; }
    public Guid UserId { get; set; }
}