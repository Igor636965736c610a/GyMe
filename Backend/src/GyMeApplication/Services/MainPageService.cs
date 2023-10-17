using System.Linq.Expressions;
using GyMeApplication.IServices;
using GyMeApplication.Models.ReactionsAndComments;
using GyMeApplication.Models.Series;
using GyMeApplication.MyMapper;
using GyMeApplication.Options;
using GyMeCore.IRepo;
using GyMeCore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GyMeApplication.Services;

public class MainPageService : IMainPageService
{
    private readonly GyMePostgresContext _gyMePostgresContext;
    private readonly IUserContextService _userContextService;
    private readonly IUserRepo _userRepo;
    private readonly IReactionRepo _reactionRepo;
    private readonly ICommentRepo _commentRepo;
    private readonly IGyMeMapper _gyMeMapper;

    public MainPageService(GyMePostgresContext gyMePostgresContext, IUserContextService userContextService, IUserRepo userRepo, IReactionRepo reactionRepo,
        ICommentRepo commentRepo, IGyMeMapper gyMeMapper)
    {
        _gyMePostgresContext = gyMePostgresContext;
        _userContextService = userContextService;
        _userRepo = userRepo;
        _reactionRepo = reactionRepo;
        _commentRepo = commentRepo;
        _gyMeMapper = gyMeMapper;
    }

    public async Task<IEnumerable<SimpleExercisePageElement>> GetNewSimpleExercisesForMainPage(int page, int size)
    {
        var userIdFromJwt = _userContextService.UserId;

        var user = await _userRepo.Get(userIdFromJwt);
        if (user is null)
            throw new InvalidProgramException("Something went wrong");

        var simpleExercisesForMainPage = await GetSimpleExercisesForMainPage(page, size, false, user);
        user.LastRefreshMainPage = DateTime.UtcNow;

        await _userRepo.Update(user);
        return simpleExercisesForMainPage;
    }

    public async Task<IEnumerable<SimpleExercisePageElement>> GetPastSimpleExercisesForMainPage(int page, int size)
    {
        var userIdFromJwt = _userContextService.UserId;

        var user = await _userRepo.Get(userIdFromJwt);
        if (user is null)
            throw new InvalidProgramException("Something went wrong");
        
        return await GetSimpleExercisesForMainPage(page, size, true, user);
    }
    
    private async Task<IEnumerable<SimpleExercisePageElement>> GetSimpleExercisesForMainPage(
        int page, int size, bool isPastExercises, User user)
    {
    
        var timeStampCondition = isPastExercises
            ? (Expression<Func<SimpleExercise, bool>>)(x => x.TimeStamp < user.LastRefreshMainPage)
            : (Expression<Func<SimpleExercise, bool>>)(x => x.TimeStamp > user.LastRefreshMainPage);
    
        var simpleExercises = await _gyMePostgresContext.UserFriends
            .Where(x => x.UserId == user.Id && x.FriendStatus == FriendStatus.Friend)
            .Include(x => x.Friend.SimpleExercises)
            .SelectMany(x => x.Friend.SimpleExercises)
            .Where(timeStampCondition)
            .OrderBy(x => x.TimeStamp)
            .Skip(page * size)
            .Take(size)
            .Include(x => x.User)
            .ThenInclude(x => x.ExtendedUser)
            .Include(x => x.Reactions
                .OrderBy(z => z.ReactionType == ReactionType.Image.ToStringFast())
                .ThenBy(z => z.TimeStamp)
                .Take(3))
            .ThenInclude(x => x.User)
            .Include(x => x.Series)
            .ToListAsync();
    
        var reactionsCount = await _reactionRepo.GetReactionsCount(simpleExercises.Select(x => x.Id));
    
        var commentsCount = await _commentRepo.GetCommentsCount(simpleExercises.Select(x => x.Id));
    
        var simpleExercisesPageElemDto = simpleExercises.Select(x => new SimpleExercisePageElement()
        {
            Id = x.Id,
            ExerciseId = x.ExerciseId,
            ExercisesTypeDto = x.ExerciseType,
            Date = x.TimeStamp,
            UserId = x.UserId,
            User = new PageElementOwner()
            {
                Id = x.User.Id,
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                UserName = x.User.UserName,
                ProfilePictureUrl = x.User.ExtendedUser!.ProfilePictureUrl
            },
            Series = x.Series.Select(z => _gyMeMapper.GetSeriesDtoMap(z)),
            FirstThreeReactionsDto = x.Reactions.Select(z => _gyMeMapper.GetReactionDtoMap(z)),
            Description = x.Description,
            ReactionsCount = reactionsCount[x.Id],
            CommentsCount = commentsCount[x.Id],
            MaxRep = x.Series.OrderByDescending(z => z.Weight).ThenByDescending(z => z.NumberOfRepetitions).First()
                .Weight,
            Score = x.Series.Sum(z => (int)Math.Round(z.Weight / (1.0278 - 0.0278 * z.NumberOfRepetitions), 2,
                MidpointRounding.AwayFromZero)),
            NumberOfRepetitions = x.Series.Sum(z => z.NumberOfRepetitions),
            NumberOfSeries = x.Series.Count,
            SumOfKilograms = x.Series.Sum(z => z.Weight),
            AverageNumberOfRepetitionsPerSeries = (int)Math.Round(x.Series.Average(z => z.NumberOfRepetitions), 2,
                MidpointRounding.AwayFromZero),
            AverageWeight = (int)Math.Round(x.Series.Average(z => z.Weight), 2, MidpointRounding.AwayFromZero),
        });
    
        return simpleExercisesPageElemDto;
    }
}

public class SimpleExercisePageElement
{
    public Guid Id { get; set; }
    public Guid ExerciseId { get; set; }
    public string ExercisesTypeDto { get; set; }
    public DateTime Date { get; set; }
    public Guid UserId { get; set; }
    public PageElementOwner User { get; set; }
    public IEnumerable<GetSeriesDto> Series { get; set; }
    public IEnumerable<GetReactionDto> FirstThreeReactionsDto { get; set; }
    public int ReactionsCount { get; set; }
    public int CommentsCount { get; set; }
    public string? Description { get; set; }
    public int? MaxRep { get; set; }
    public int? Score { get; set; }
    public int? NumberOfRepetitions { get; set; }
    public int? NumberOfSeries { get; set; }
    public int? SumOfKilograms { get; set; }
    public int? AverageNumberOfRepetitionsPerSeries { get; set; }
    public int? AverageWeight { get; set; }
}

public class PageElementOwner
{
    public Guid Id { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string ProfilePictureUrl { get; set; }
}

