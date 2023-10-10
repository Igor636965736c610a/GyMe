namespace GyMeInfrastructure.Models.ReactionsAndComments;

public class GetCommentReactionCount
{
    public string ReactionType { get; set; }
    public string Emoji { get; set; }
    public int Count { get; set; }
}