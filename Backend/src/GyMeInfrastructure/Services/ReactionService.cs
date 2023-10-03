using System.Configuration.Provider;
using System.Transactions;
using GyMeCore.IRepo;
using GyMeCore.Models.Entities;
using GyMeInfrastructure.Models.ReactionsAndComments.BodyRequest;
using GyMeInfrastructure.Exceptions;
using GyMeInfrastructure.IServices;
using GyMeInfrastructure.Models.ReactionsAndComments;
using GyMeInfrastructure.MyMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace GyMeInfrastructure.Services;

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

    public async Task AddEmojiReaction(Guid simpleExerciseId, ReactionType reactionType)
    {
        var userIdFromJwt = _userContextService.UserId;

        var simpleExercise = await _simpleExerciseRepo.Get(simpleExerciseId);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");
        
        if (simpleExercise.UserId == userIdFromJwt)
            throw new InvalidOperationException("You cant add a reaction to own post");
        
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, simpleExercise.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        string? imageUrl = null!;
        if (reactionType == ReactionType.Image)
        {
            var resourcesAddresses = await _userRepo.GetResourcesAddresses(userIdFromJwt);
            if (resourcesAddresses?.ReactionImageUrl is null)
                throw new InvalidOperationException("No set photo for reaction");
            imageUrl = resourcesAddresses.ReactionImageUrl;
        }
            
        var existingReaction = await _reactionRepo.Get(simpleExerciseId, userIdFromJwt);
        if (existingReaction is not null)
        {
            existingReaction.ImageUrl = imageUrl;
            existingReaction.Emoji = GetEmoji(reactionType);
            existingReaction.ReactionType = reactionType.ToStringFast();
            existingReaction.TimeStamp = DateTime.UtcNow;

            await _reactionRepo.Update(existingReaction);
            return;
        }

        var emojiReaction = new Reaction(Guid.NewGuid(), GetEmoji(reactionType), imageUrl, reactionType.ToStringFast(),
            simpleExercise.Id, userIdFromJwt);

        await _reactionRepo.Create(emojiReaction);
    }
    
    public async Task SetImageReaction(IFormFile image)
    {
        var userIdFromJwt = _userContextService.UserId;

        var resourcesAddresses = await _userRepo.GetResourcesAddresses(userIdFromJwt);
        var newResourcesAddressesId = Guid.NewGuid();
        var reactionImageUrl = _gyMeResourceService.GenerateUrlToPhoto(userIdFromJwt.ToString(), newResourcesAddressesId.ToString());
        if (resourcesAddresses is null)
        {
            resourcesAddresses = new ResourcesAddresses(newResourcesAddressesId, reactionImageUrl, userIdFromJwt);
            await _userRepo.AddResourcesAddresses(resourcesAddresses);
        }
        else
        {
            resourcesAddresses.Id = newResourcesAddressesId;
            resourcesAddresses.ReactionImageUrl = reactionImageUrl;
        }

        var path = _gyMeResourceService.GeneratePathToPhoto(newResourcesAddressesId + Path.GetExtension(image.FileName), userIdFromJwt.ToString());
        await _gyMeResourceService.SaveImageOnServer(image, path);
    }

    public async Task<IEnumerable<GetReactionDto>> GetReactions(Guid simpleExerciseId, int page, int size, ReactionType? reactionType)
    {
        var userIdFromJwt = _userContextService.UserId;

        var simpleExercise = await _simpleExerciseRepo.Get(simpleExerciseId);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");
        
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, simpleExercise.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        var reactionsSource = _reactionRepo.GetAll(simpleExercise.Id, page, size);
        if (reactionType is null)
        {
            var reactions = await reactionsSource
                .OrderBy(x => x.ReactionType == ReactionType.Image.ToStringFast())
                .ThenBy(x => x.ReactionType)
                .ThenBy(x => x.TimeStamp)
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
                .OrderBy(x => x.TimeStamp)
                .Skip(page * size)
                .Take(size)
                .ToListAsync();
            
            var reactionsDto = reactions.Select(x => _gyMeMapper.GetReactionDtoMap(x));

            return reactionsDto;
        }
    }

    public async Task<IEnumerable<GetReactionCountDto>> GetSpecificReactionsCount(Guid simpleExerciseId)
    {
        var userIdFromJwt = _userContextService.UserId;

        var simpleExercise = await _simpleExerciseRepo.Get(simpleExerciseId);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");
        
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, simpleExercise.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        var reactionsCount = await _reactionRepo.GetSpecificReactionsCount(simpleExerciseId);

        var reactionsCountDto = reactionsCount.Select(x => new GetReactionCountDto()
        {
            ReactionType = x.ReactionType,
            Emoji = x.Emoji,
            Count = x.Count
        });

        return reactionsCountDto;
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

    private static string? GetEmoji(ReactionType reactionType) => reactionType switch
    {
        ReactionType.Image => null,
        _ => throw new InvalidProgramException("Invalid Server Error")
    };
}