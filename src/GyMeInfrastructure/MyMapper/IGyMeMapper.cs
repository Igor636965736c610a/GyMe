using GymAppCore.Models.Entities;
using GymAppInfrastructure.Models.ReactionsAndComments;
using GymAppInfrastructure.Models.Series;
using GymAppInfrastructure.Models.SimpleExercise;
using GymAppInfrastructure.Models.User;

namespace GymAppInfrastructure.MyMapper;

public interface IGyMeMapper
{
    GetCommentDto MaGetCommentDtoMap(Comment comment, Dictionary<string, int> reactionsCount);
    GetCommentReactionDto GetCommentReactionDtoMap(CommentReaction commentReaction);
    GetReactionDto GetReactionDtoMap(Reaction reaction);
    GetSeriesDto GetSeriesDtoMap(Series series);
    GetSimpleExerciseDto GetSimpleExerciseDtoMap(SimpleExercise simpleExercise);
    GetUserDto GetUserDtoMap(User user, FriendStatus? friendStatus = null);
    IEnumerable<GetUserDto> GetUserDtoMap(IEnumerable<UserFriend> userFriends);
}