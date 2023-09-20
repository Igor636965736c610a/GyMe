namespace GymAppInfrastructure.Models.ReactionsAndComments;

public class GetCommentDto
{
    public Guid Id { get; set; }
    public Guid SimpleExerciseId { get; set; }
    public string Message { get; set; }
    public Owner User { get; set; }
    public DateTime TimeStamp { get; set; }
    public IEnumerable<GetReactionCountDto> GetCommentReactionCountDtos { get; set; }
}