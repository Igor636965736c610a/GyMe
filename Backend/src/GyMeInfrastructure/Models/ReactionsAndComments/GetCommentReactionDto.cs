namespace GymAppInfrastructure.Models.ReactionsAndComments;

public class GetCommentReactionDto
{
    public Guid Id { get; set; }
    public string Emoji { get; set; }
    public Owner User { get; set; }
    public Guid CommentId { get; set; }
    public string ReactionTypeDto { get; set; }
    public DateTime TimeStamp { get; set; }
}