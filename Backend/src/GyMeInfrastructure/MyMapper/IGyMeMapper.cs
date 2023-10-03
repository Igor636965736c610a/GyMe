using GyMeCore.Models.Entities;
using GyMeInfrastructure.Models.ReactionsAndComments.BodyRequest;
using GyMeInfrastructure.Models.ReactionsAndComments;
using GyMeInfrastructure.Models.Series;
using GyMeInfrastructure.Models.SimpleExercise;
using GyMeInfrastructure.Models.User;

namespace GyMeInfrastructure.MyMapper;

public interface IGyMeMapper
{
    GetCommentDto GetCommentDtoMap(Comment comment, int reactionsCount);
    GetCommentReactionDto GetCommentReactionDtoMap(CommentReaction commentReaction);
    GetReactionDto GetReactionDtoMap(Reaction reaction);
    GetSeriesDto GetSeriesDtoMap(Series series);
    GetSimpleExerciseDto GetSimpleExerciseDtoMap(SimpleExercise simpleExercise, int reactionsCount, int commentsCount);
    GetUserDto GetUserDtoMap(User user, FriendStatus? friendStatus = null);
    IEnumerable<GetUserDto> GetUserDtoMap(IEnumerable<UserFriend> userFriends);
}