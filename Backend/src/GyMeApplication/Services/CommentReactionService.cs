using GyMeApplication.Exceptions;
using GyMeApplication.IServices;
using GyMeApplication.Models.ReactionsAndComments;
using GyMeApplication.Models.ReactionsAndComments.BodyRequest;
using GyMeApplication.MyMapper;
using GyMeCore.IRepo;
using GyMeCore.Models.Entities;

namespace GyMeApplication.Services;

internal class CommentReactionService : ICommentReactionService
{
    private readonly IUserContextService _userContextService;
    private readonly ICommentRepo _commentRepo;
    private readonly ICommentReactionRepo _commentReactionRepo;
    private readonly IGyMeMapper _gyMeMapper;
    private readonly IUserRepo _userRepo;

    public CommentReactionService(IUserContextService userContextService, ICommentRepo commentRepo, ICommentReactionRepo commentReactionRepo, IGyMeMapper gyMeMapper, 
        IUserRepo userRepo)
    {
        _userContextService = userContextService;
        _commentRepo = commentRepo;
        _commentReactionRepo = commentReactionRepo;
        _gyMeMapper = gyMeMapper;
        _userRepo = userRepo;
    }

    public async Task<Guid> AddCommentReaction(PostCommentReactionDto postCommentReactionDto)
    {
        var userIdFromJwt = _userContextService.UserId;

        var comment = await _commentRepo.Get(postCommentReactionDto.CommentId);
        if (comment is null)
            throw new NullReferenceException("Comment does not exist");

        var commentReaction = await _commentReactionRepo.Get(userIdFromJwt, postCommentReactionDto.CommentId);
        var emoji = GetEmoji(postCommentReactionDto.CommentReactionType);

        if (commentReaction is not null)
        {
            commentReaction.Emoji = emoji;
            commentReaction.ReactionType = postCommentReactionDto.CommentReactionType.ToStringFast();
            commentReaction.TimeStamp = DateTime.UtcNow;
            
            await _commentReactionRepo.Update(commentReaction);
            return commentReaction.Id;
        }
        else
        {
            var newCommentReaction = new CommentReaction(Guid.NewGuid(), postCommentReactionDto.CommentReactionType.ToStringFast(),
                emoji, postCommentReactionDto.CommentId, userIdFromJwt);

            await _commentReactionRepo.Create(newCommentReaction);
            return newCommentReaction.Id;
        }
    }

    public async Task<IEnumerable<GetCommentReactionDto>> GetCommentsReactions(Guid commentId, CommentReactionType? commentReactionType, int page, int size)
    {
        var userIdFromJwt = _userContextService.UserId;

        var comment = await _commentRepo.Get(commentId);
        if (comment is null)
            throw new NullReferenceException("Comment does not exist");
        
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, comment.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        var commentReactionsSource = _commentReactionRepo.GetAll(commentId);

        if(commentReactionType is null)
        {
            var commentReactions = commentReactionsSource
                .OrderBy(x => x.ReactionType)
                .ThenBy(x => x.TimeStamp)
                .Skip(page * size)
                .Take(size);

            return commentReactions.Select(x => _gyMeMapper.GetCommentReactionDtoMap(x));
        }
        else
        {
            var commentReactions = commentReactionsSource
                .Where(x => x.ReactionType == commentReactionType.ToString())
                .OrderBy(x => x.TimeStamp)
                .Skip(page * size)
                .Take(size);

            return commentReactions.Select(x => _gyMeMapper.GetCommentReactionDtoMap(x));
        }
    }

    public async Task<IEnumerable<GetCommentReactionCount>> GetSpecificCommentReactionCount(Guid commentId)
    {
        var userIdFromJwt = _userContextService.UserId;

        var comment = await _commentRepo.Get(commentId);
        if (comment is null)
            throw new NullReferenceException("Comment does not exist");
        
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, comment.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");
        
        var commentReactionsCount = await _commentReactionRepo.GetSpecificCommentReactionsCount(commentId);

        var commentReactionsCountDto = commentReactionsCount.Select(x => new GetCommentReactionCount()
        {
            ReactionType = x.ReactionType,
            Emoji = x.Emoji,
            Count = x.Count
        });

        return commentReactionsCountDto;
    }

    public async Task RemoveCommentReaction(Guid commentReactionId)
    {
        var userIdFromJwt = _userContextService.UserId;

        var commentReaction = await _commentReactionRepo.Get(commentReactionId);
        if(commentReaction is null)
            throw new NullReferenceException("Not Found");
        
        if (commentReaction.UserId != userIdFromJwt)
            throw new ForbiddenException("You do not have the appropriate permissions");

        await _commentReactionRepo.Remove(commentReaction);
    }
    
    private static string GetEmoji(CommentReactionType commentReactionType) => commentReactionType switch
    {
        CommentReactionType.HeartEyes => "dsadwqd",
        _ => throw new InvalidProgramException("Invalid Server Error")
    };
}