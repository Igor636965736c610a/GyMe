using GyMeCore.IRepo;
using GyMeCore.Models.Entities;
using GyMeInfrastructure.Models.ReactionsAndComments;
using GyMeInfrastructure.Models.Series;
using GyMeInfrastructure.Models.SimpleExercise;
using GyMeInfrastructure.Models.User;

namespace GyMeInfrastructure.MyMapper;

public class GyMeMapper : IGyMeMapper
{
    public GetCommentDto GetCommentDtoMap(Comment comment, int reactionsCount)
        => new GetCommentDto()
        {
            Id = comment.Id,
            SimpleExerciseId = comment.SimpleExerciseId,
            Message = comment.Message,
            User = new Owner()
            {
                Id = comment.UserId,
                UserName = comment.User.UserName
            },
            TimeStamp = comment.TimeStamp,
            FirstThreeCommentReactionsDto = comment.CommentReactions.Select(x => GetCommentReactionDtoMap(x)),
            ReactionsCount = reactionsCount
        };

    public GetCommentReactionDto GetCommentReactionDtoMap(CommentReaction commentReaction)
        => new GetCommentReactionDto()
        {
            Id = commentReaction.Id,
            CommentId = commentReaction.CommentId,
            Emoji = commentReaction.Emoji,
            ReactionTypeDto = commentReaction.ReactionType,
            TimeStamp = commentReaction.TimeStamp,
            User = new Owner()
            {
                Id = commentReaction.UserId,
                UserName = commentReaction.User.UserName
            }
        };
    
    public GetReactionDto GetReactionDtoMap(Reaction reaction)
        => new GetReactionDto()
        {
            Id = reaction.Id,
            Emoji = reaction.Emoji,
            ImageReaction = reaction.ImageUrl,
            User = new Owner()
            {
                Id = reaction.UserId,
                UserName = reaction.User.UserName
            },
            SimpleExerciseId = reaction.SimpleExerciseId,
            ReactionTypeDto = reaction.ReactionType,
            TimeStamp = reaction.TimeStamp
        };
    
    public GetSeriesDto GetSeriesDtoMap(Series series)
    {
        return new GetSeriesDto()
        {
            Id = series.Id,
            NumberOfRepetitions = series.NumberOfRepetitions,
            Weight = series.Weight
        };
    }
    
    public GetSimpleExerciseDto GetSimpleExerciseDtoMap(SimpleExercise simpleExercise, int reactionsCount, int commentsCount)
        => new GetSimpleExerciseDto()
        {
            Id = simpleExercise.Id,
            Date = simpleExercise.TimeStamp,
            ExerciseId = simpleExercise.ExerciseId,
            ExercisesTypeDto = simpleExercise.ExerciseType,
            Series = simpleExercise.Series.Select(x => GetSeriesDtoMap(x)),
            UserId = simpleExercise.UserId,
            Description = simpleExercise.Description,
            ReactionsCount = reactionsCount,
            CommentsCount = commentsCount,
            MaxRep = simpleExercise.Series.OrderByDescending(x => x.Weight).ThenByDescending(x => x.NumberOfRepetitions).First().Weight,
            Score = simpleExercise.Series.Sum(x => (int)Math.Round(x.Weight / (1.0278 - 0.0278 * x.NumberOfRepetitions), 2, MidpointRounding.AwayFromZero)),
            NumberOfRepetitions = simpleExercise.Series.Sum(x => x.NumberOfRepetitions),
            NumberOfSeries = simpleExercise.Series.Count,
            SumOfKilograms = simpleExercise.Series.Sum(x => x.Weight),
            AverageNumberOfRepetitionsPerSeries = (int)Math.Round(simpleExercise.Series.Average(x => x.NumberOfRepetitions), 2, MidpointRounding.AwayFromZero),
            AverageWeight = (int)Math.Round(simpleExercise.Series.Average(x => x.Weight), 2, MidpointRounding.AwayFromZero),
            FirstThreeReactionsDto = simpleExercise.Reactions.Select(x => GetReactionDtoMap(x))
        };
    
    public GetUserDto GetUserDtoMap(User user, FriendStatus? friendStatus = null)
        => new GetUserDto()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            PrivateAccount = user.ExtendedUser.PrivateAccount,
            Gender = user.ExtendedUser.Gender,
            ProfilePictureUrl = user.ExtendedUser.ProfilePictureUrl,
            Description = user.ExtendedUser.Description,
            FriendStatus = friendStatus?.ToStringFast()
        };

    public IEnumerable<GetUserDto> GetUserDtoMap(IEnumerable<UserFriend> userFriends)
        => userFriends.Select(x => GetUserDtoMap(x.Friend, x.FriendStatus));
}