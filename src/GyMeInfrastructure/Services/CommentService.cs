using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Exceptions;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Models.ReactionsAndComments.BodyRequest;

namespace GymAppInfrastructure.Services;

internal class CommentService : ICommentService
{
    private readonly IUserContextService _userContextService;
    private readonly ISimpleExerciseRepo _simpleExerciseRepo;
    private readonly ICommentRepo _commentRepo;
    private readonly IUserRepo _userRepo;

    public CommentService(IUserContextService userContextService, ISimpleExerciseRepo simpleExerciseRepo, ICommentRepo commentRepo, IUserRepo userRepo)
    {
        _userContextService = userContextService;
        _simpleExerciseRepo = simpleExerciseRepo;
        _commentRepo = commentRepo;
        _userRepo = userRepo;
    }

    public async Task AddComment(PostCommentDto postCommentDto)
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
    }

    public async Task Get(Guid commentId)
    {
        var userIdFromJwt = _userContextService.UserId;

        var comment = await _commentRepo.Get(commentId);
        
        if(comment is null)
            throw new NullReferenceException("Not Found");
        
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, comment.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");
        
        
    }
}