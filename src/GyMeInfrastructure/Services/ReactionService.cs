using System.Configuration.Provider;
using System.Transactions;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Exceptions;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Models.ReactionsAndComments;
using GymAppInfrastructure.MyMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Stripe.Issuing;

namespace GymAppInfrastructure.Services;

internal class ReactionService : IReactionService
{
    private readonly IUserContextService _userContextService;
    private readonly IUserRepo _userRepo;
    private readonly ISimpleExerciseRepo _simpleExerciseRepo;
    private readonly IReactionRepo _reactionRepo;
    private readonly IGyMeMapper _gyMeMapper;
    private readonly IGyMeResourceService _gyMeResourceService;

    public ReactionService(IUserContextService userContextService, IUserRepo userRepo, ISimpleExerciseRepo simpleExerciseRepo, IReactionRepo reactionRepo, IGyMeMapper gyMeMapper, 
        IGyMeResourceService gyMeResourceService)
    {
        _userContextService = userContextService;
        _userRepo = userRepo;
        _simpleExerciseRepo = simpleExerciseRepo;
        _reactionRepo = reactionRepo;
        _gyMeMapper = gyMeMapper;
        _gyMeResourceService = gyMeResourceService;
    }

    public async Task AddEmojiReaction(Guid simpleExerciseId, Emoji emoji)
    {
        var userIdFromJwt = _userContextService.UserId;

        var simpleExercise = await _simpleExerciseRepo.Get(simpleExerciseId);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");
        
        if (simpleExercise.UserId == userIdFromJwt)
            throw new InvalidOperationException("You cant add a reaction to own post");
        
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, simpleExercise.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");
        
        var existingReaction = await _reactionRepo.Get(simpleExerciseId, userIdFromJwt);
        if (existingReaction is not null)
        {
            existingReaction.ImageUel = null;
            existingReaction.Emoji = GetEmoji(emoji);
            existingReaction.ReactionType = emoji.ToStringFast();
            existingReaction.TimeStamp = DateTime.UtcNow;

            await _reactionRepo.Update(existingReaction);
            return;
        }

        var emojiReaction = new Reaction(Guid.NewGuid(), null, GetEmoji(emoji), emoji.ToStringFast(),
            simpleExercise.Id, userIdFromJwt);

        await _reactionRepo.Create(emojiReaction);
    }
    
    public async Task AddImageReaction(Guid simpleExerciseId, IFormFile image)
    {
        var userIdFromJwt = _userContextService.UserId;

        var simpleExercise = await _simpleExerciseRepo.Get(simpleExerciseId);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");
        
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, simpleExercise.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");
        
        var existingReaction = await _reactionRepo.Get(simpleExerciseId, userIdFromJwt);
        if (existingReaction is not null)
        {
            var path = _gyMeResourceService.GeneratePathToPhoto(existingReaction.Id.ToString(), userIdFromJwt + Path.GetExtension(image.FileName));
            existingReaction.ImageUel = path;
            existingReaction.Emoji = null;
            existingReaction.ReactionType = ReactionType.Image.ToStringFast();
            existingReaction.TimeStamp = DateTime.UtcNow;

            await _reactionRepo.Update(existingReaction);
            await _gyMeResourceService.SaveImageOnServer(image, path);

            return;
        }

        var newEmojiId = Guid.NewGuid();
        var newPath = _gyMeResourceService.GeneratePathToPhoto(newEmojiId.ToString(), userIdFromJwt + Path.GetExtension(image.FileName));
        var emojiReaction = new Reaction(newEmojiId, null, newPath, ReactionType.Image.ToStringFast(),
            simpleExercise.Id, userIdFromJwt);

        using var scope = new TransactionScope();
        await _reactionRepo.Create(emojiReaction);
        await _gyMeResourceService.SaveImageOnServer(image, newPath);
        scope.Complete();
    }

    public async Task<IEnumerable<GetReactionDto>> GetReactions(Guid simpleExerciseId, int page, int size, ReactionType? reactionType)
    {
        var userIdFromJwt = _userContextService.UserId;

        var simpleExercise = await _simpleExerciseRepo.Get(simpleExerciseId);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");
        
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, simpleExercise.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        var reactionsSource = await _reactionRepo.GetAll(simpleExercise.Id, page, size);
        if (reactionType is null)
        {
            var reactions = await reactionsSource
                .OrderBy(x => x.ReactionType == ReactionType.Image.ToStringFast())
                .ThenBy(x => x.ReactionType)
                .Skip(page * size)
                .Take(size)
                .ToListAsync();

            var reactionsDto = reactions.Select(x => _gyMeMapper.GetReactionDtoMap(x));

            return reactionsDto;
        }
        else
        {
            var reactions = await reactionsSource
                .Where(x => x.ReactionType == reactionType.ToString())
                .Skip(page * size)
                .Take(size)
                .ToListAsync();
            
            var reactionsDto = reactions.Select(x => _gyMeMapper.GetReactionDtoMap(x));

            return reactionsDto;
        }
    }

    public async Task RemoveReaction(Guid reactionId)
    {
        var userIdFromJwt = _userContextService.UserId;

        var reaction = await _reactionRepo.Get(reactionId);
        if(reaction is null)
            throw new NullReferenceException("Not Found");
        
        if (reaction.UserId != userIdFromJwt)
            throw new ForbiddenException("You do not have the appropriate permissions");

        await _reactionRepo.Remove(reaction);
    }

    private static string GetEmoji(Emoji emoji) => emoji switch
    {
        _ => throw new InvalidProgramException("Invalid Server Error")
    };
}