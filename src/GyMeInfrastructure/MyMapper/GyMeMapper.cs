using GymAppCore.Models.Entities;
using GymAppInfrastructure.Models.ReactionsAndComments;
using GymAppInfrastructure.Models.Series;
using GymAppInfrastructure.Models.SimpleExercise;
using GymAppInfrastructure.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace GymAppInfrastructure.MyMapper;

public class GyMeMapper : IGyMeMapper
{
    public GetCommentDto MaGetCommentDtoMap(Comment comment, Dictionary<string, int> reactionsCount)
        => new GetCommentDto()
        {
            Id = comment.Id,
            SimpleExerciseId = comment.SimpleExerciseId,
            Message = comment.Message,
            User = new Owner()
            {
                Id = comment.User.Id,
                UserName = comment.User.UserName
            },
            TimeStamp = comment.TimeStamp,
            GetCommentReactionCountDtos = reactionsCount.Select(x => new GetReactionCountDto(){ ReactionType = x.Key, Count = x.Value })
        };
    
    public GetCommentReactionDto GetCommentReactionDtoMap(CommentReaction commentReaction)
        => new GetCommentReactionDto()
        {
            Id = commentReaction.Id,
            Emoji = commentReaction.Emoji,
            User = new Owner()
            {
                Id = commentReaction.User.Id,
                UserName = commentReaction.User.UserName
            },
            CommentId = commentReaction.CommentId,
            TimeStamp = commentReaction.TimeStamp
        };
    
    public GetReactionDto GetReactionDtoMap(Reaction reaction)
        => new GetReactionDto()
        {
            Id = reaction.Id,
            Emoji = reaction.Emoji,
            ImageReaction = reaction.ImageUel,
            User = new Owner()
            {
                Id = reaction.User.Id,
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
    
    public GetSimpleExerciseDto GetSimpleExerciseDtoMap(SimpleExercise simpleExercise)
        => new GetSimpleExerciseDto()
        {
            Id = simpleExercise.Id,
            Date = simpleExercise.Date,
            ExerciseId = simpleExercise.ExerciseId,
            Series = simpleExercise.Series.Select(x => GetSeriesDtoMap(x)),
            Description = simpleExercise.Description,
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
            Gender = (GenderDto)user.ExtendedUser.Gender,
            ProfilePictureUrl = user.ExtendedUser.ProfilePictureUrl,
            Description = user.ExtendedUser.Description,
            FriendStatus = friendStatus.HasValue ? (FriendStatusDto)friendStatus : null
        };

    public IEnumerable<GetUserDto> GetUserDtoMap(IEnumerable<UserFriend> userFriends)
        => userFriends.Select(x => GetUserDtoMap(x.Friend, x.FriendStatus));
}