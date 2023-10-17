namespace GyMeApplication.Models.ReactionsAndComments;

public class GetReactionCountDto
{
    public string ReactionType { get; set; }
    public string? Emoji { get; set; }
    public int Count { get; set; }
}