using GyMeApplication.Models.Account;
using GyMeApplication.Models.Exercise;
using GyMeApplication.Models.ReactionsAndComments;
using GyMeApplication.Models.Series;
using GyMeApplication.Models.SimpleExercise;
using GyMeApplication.Models.User;
using GyMeCore.Models.Entities;
using GyMeCore.Models.Results;

namespace GyMeApplication.MyMapper;

public interface IGyMeMapper
{
    CommonFriendsResultDto GetCommonFriendsResultDtoMap(CommonFriendsResult commonFriendsResult);
    GetAccountInfModel GetAccountInfModelDtoMap(User user);
    GetCommentDto GetCommentDtoMap(Comment comment, int reactionsCount);
    GetCommentReactionDto GetCommentReactionDtoMap(CommentReaction commentReaction);
    GetReactionDto GetReactionDtoMap(Reaction reaction);
    GetSeriesDto GetSeriesDtoMap(Series? series);
    GetExerciseDto GetExerciseDtoMap(Exercise exercise, GetSeriesDto? maxRepSeries);
    GetSimpleExerciseDto GetSimpleExerciseDtoMap(SimpleExercise simpleExercise, int reactionsCount, int commentsCount);
    GetUserDto GetUserDtoMap(User user, FriendStatus? friendStatus = null);
}