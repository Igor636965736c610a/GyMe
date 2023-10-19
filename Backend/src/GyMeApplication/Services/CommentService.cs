using GyMeApplication.Exceptions;
using GyMeApplication.IServices;
using GyMeApplication.Models.ReactionsAndComments;
using GyMeApplication.Models.ReactionsAndComments.BodyRequest;
using GyMeApplication.MyMapper;
using GyMeCore.IRepo;
using GyMeCore.Models.Entities;

namespace GyMeApplication.Services;

internal class CommentService : ICommentService
{
    private readonly IUserContextService _userContextService;
    private readonly ISimpleExerciseRepo _simpleExerciseRepo;
    private readonly ICommentRepo _commentRepo;
    private readonly IUserRepo _userRepo;
    private readonly IGyMeMapper _gyMeMapper;
    private readonly ICommentReactionRepo _commentReactionRepo;

    public CommentService(IUserContextService userContextService, ISimpleExerciseRepo simpleExerciseRepo, ICommentRepo commentRepo, IUserRepo userRepo, IGyMeMapper gyMeMapper
        ,ICommentReactionRepo commentReactionRepo)
    {
        _userContextService = userContextService;
        _simpleExerciseRepo = simpleExerciseRepo;
        _commentRepo = commentRepo;
        _userRepo = userRepo;
        _gyMeMapper = gyMeMapper;
        _commentReactionRepo = commentReactionRepo;
    }

    public async Task<Guid> AddComment(PostCommentDto postCommentDto)
    {
        var userIdFromJwt = _userContextService.UserId;

        var simpleExercise = await _simpleExerciseRepo.Get(postCommentDto.SimpleExerciseId);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");
     
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, simpleExercise.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");
        
        var comment = new Comment(Guid.NewGuid(), postCommentDto.Message, postCommentDto.SimpleExerciseId,
            userIdFromJwt);

        await _commentRepo.Create(comment);
        return comment.Id;
    }

    public async Task<GetCommentDto> GetComment(Guid commentId)
    {
        var userIdFromJwt = _userContextService.UserId;

        var comment = await _commentRepo.Get(commentId);
        
        if(comment is null)
            throw new NullReferenceException("Not Found");
        
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, comment.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        var commentReactionsCount = await _commentReactionRepo.GetCommentReactionsCount(commentId);
        var commentDto = _gyMeMapper.GetCommentDtoMap(comment, commentReactionsCount);

        return commentDto;
    }
    
    public async Task<IEnumerable<GetCommentDto>> GetCommentsSortedByPubTime(Guid simpleExerciseId, int page, int size)
    {
        var userIdFromJwt = _userContextService.UserId;

        var simpleExercise = await _simpleExerciseRepo.Get(simpleExerciseId);
        if(simpleExercise is null)
            throw new InvalidOperationException("SimpleExercise does not exist");
                
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, simpleExercise.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");
        
        var comments = await _commentRepo.GetSortedByTimeStamp(simpleExerciseId, page, size);
        var commentReactionsCount = await _commentReactionRepo.GetCommentReactionsCount(comments.Select(x => x.Id));
        var commentsDto = comments.Select(x => _gyMeMapper.GetCommentDtoMap(x, commentReactionsCount[x.Id]));

        return commentsDto;
    }
    
    public async Task<IEnumerable<GetCommentDto>> GetCommentsSortedByReactionsCount(Guid simpleExerciseId, int page, int size)
    {
        var userIdFromJwt = _userContextService.UserId;

        var simpleExercise = await _simpleExerciseRepo.Get(simpleExerciseId);
        if(simpleExercise is null)
            throw new InvalidOperationException("SimpleExercise does not exist");
                
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, simpleExercise.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");
        
        var comments = await _commentRepo.GetSortedByReactionsCount(simpleExerciseId, page, size);
        var commentReactionsCount = await _commentReactionRepo.GetCommentReactionsCount(comments.Select(x => x.Id));
        var commentsDto = comments.Select(x => _gyMeMapper.GetCommentDtoMap(x, commentReactionsCount[x.Id]));

        return commentsDto;
    }

    public async Task UpdateComment(PutCommentDto putCommentDto, Guid commentId)
    {
        var userIdFromJwt = _userContextService.UserId;

        var comment = await _commentRepo.Get(commentId);
        if (comment is null)
            throw new NullReferenceException("Not Found");
        
        if(userIdFromJwt != comment.Id)
            throw new ForbiddenException("You do not have the appropriate permissions");

        comment.Message = putCommentDto.Message;

        await _commentRepo.Update(comment);
    }

    public async Task RemoveComment(Guid commentId)
    {
        var userIdFromJwt = _userContextService.UserId;

        var comment = await _commentRepo.Get(commentId);
        if (comment is null)
            throw new NullReferenceException("Not Found");
        
        if(userIdFromJwt != comment.Id)
            throw new ForbiddenException("You do not have the appropriate permissions");

        await _commentRepo.Remove(comment);
    }
}