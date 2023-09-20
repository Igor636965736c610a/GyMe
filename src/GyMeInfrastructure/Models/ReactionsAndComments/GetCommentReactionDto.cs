namespace GymAppInfrastructure.Models.ReactionsAndComments;

public class GetCommentReactionDto
{
    public Guid Id { get; set; }
    public string Emoji { get; set; }
    public DateTime TimeStamp { get; set; }
    public Guid CommentId { get; set; }
    public Owner User { get; set; }
}