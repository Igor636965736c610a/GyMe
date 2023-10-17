namespace GyMeApplication.Models.ReactionsAndComments.BodyRequest;

public class PostCommentReactionDto
{
    public Guid CommentId { get; set; }
    public CommentReactionType CommentReactionType { get; set; }
}