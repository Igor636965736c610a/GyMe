namespace GyMeCore.Models.Results;

public class ReactionsCountResult
{
    public string ReactionType { get; set; }
    public string? Emoji { get; set; }
    public int Count { get; set; }
}