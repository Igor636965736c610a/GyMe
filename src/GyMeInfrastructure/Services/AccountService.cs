using System.Transactions;
using AutoMapper;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Models.Account;
using GymAppInfrastructure.Models.User;
using GymAppInfrastructure.Exceptions;
using GymAppInfrastructure.IServices;
using Microsoft.AspNetCore.Http;

namespace GymAppInfrastructure.Services;

internal class AccountService : IAccountService
{
    private readonly IUserRepo _userRepo;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    private readonly IGyMeResourceService _gyMeResourceService;

    public AccountService(IUserRepo userRepo, IMapper mapper, IUserContextService userContextService, IGyMeResourceService gyMeResourceService)
    {
        _userRepo = userRepo;
        _mapper = mapper;
        _userContextService = userContextService;
        _gyMeResourceService = gyMeResourceService;
    }

    public async Task<GetAccountInfModel> GetInf()
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var user = await _userRepo.Get(userIdFromJwt);
        if (user is null)
            throw new InvalidProgramException("Something went wrong");

        var accountInf = _mapper.Map<User, GetAccountInfModel>(user);
        
        if (user.ExtendedUser is not null)
        {
            accountInf.Gender = (GenderDto)user.ExtendedUser.Gender;
            accountInf.ProfilePictureUrl = user.ExtendedUser.ProfilePictureUrl;
        }
        
        return accountInf;
    }

    public async Task Update(PutUserDto putUserDto)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var user = await _userRepo.GetOnlyValid(userIdFromJwt);
        if (user?.ExtendedUser is null)
            throw new InvalidProgramException("Something went wrong");
        
        user.UserName = putUserDto.UserName;
        user.FirstName = putUserDto.FirstName;
        user.LastName = putUserDto.LastName;
        user.ExtendedUser.Description = putUserDto.Description;
        user.ExtendedUser.PrivateAccount = putUserDto.PrivateAccount;
        
        await _userRepo.Update(user);
    }
    
    public async Task SetUserProfile(IFormFile image)
    {
        var userIdFromJwt = _userContextService.UserId;

        var user = await _userRepo.GetOnlyValid(userIdFromJwt);

        if (user?.ExtendedUser is null)
            throw new InvalidProgramException("Something went wrong");

        var url = _gyMeResourceService.GenerateUrlToPhoto(user.Id.ToString(), user.Id + Path.GetExtension(image.FileName));
        user.ExtendedUser.ProfilePictureUrl = url;
        
        await _userRepo.Update(user);
        var path = _gyMeResourceService.GeneratePathToPhoto(user.Id + Path.GetExtension(image.FileName),
            user.Id.ToString());
        
        await _gyMeResourceService.SaveImageOnServer(image, path);
    }

    public async Task Remove()
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var user = await _userRepo.GetOnlyValid(userIdFromJwt);
        if (user is null)
            throw new InvalidProgramException("Something went wrong");

        await _userRepo.RemoveUser(user);
        _gyMeResourceService.RemoveProfilePicture(userIdFromJwt.ToString());
    }
}