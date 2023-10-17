namespace GyMeApplication.Models.ReactionsAndComments;

public class GetCommentDto
{
    public Guid Id { get; set; }
    public Guid SimpleExerciseId { get; set; }
    public string Message { get; set; }
    public Owner User { get; set; }
    public DateTime TimeStamp { get; set; }
    public int ReactionsCount { get; set; }
    public IEnumerable<GetCommentReactionDto> FirstThreeCommentReactionsDto { get; set; }
}