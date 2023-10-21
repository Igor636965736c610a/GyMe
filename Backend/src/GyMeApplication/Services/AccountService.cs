using System.Transactions;
using AutoMapper;
using GyMeApplication.IServices;
using GyMeApplication.Models.Account;
using GyMeApplication.Models.User;
using GyMeCore.IRepo;
using GyMeCore.Models.Entities;
using GyMeApplication.Exceptions;
using GyMeApplication.MyMapper;
using Microsoft.AspNetCore.Http;

namespace GyMeApplication.Services;

internal class AccountService : IAccountService
{
    private readonly IUserRepo _userRepo;
    private readonly IUserContextService _userContextService;
    private readonly IGyMeResourceService _gyMeResourceService;
    private readonly IGyMeMapper _gyMeMapper;

    public AccountService(IUserRepo userRepo, IUserContextService userContextService, IGyMeResourceService gyMeResourceService, IGyMeMapper gyMeMapper)
    {
        _userRepo = userRepo;
        _userContextService = userContextService;
        _gyMeResourceService = gyMeResourceService;
        _gyMeMapper = gyMeMapper;
    }

    public async Task<GetAccountInfModel> GetInf()
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var user = await _userRepo.Get(userIdFromJwt);
        if (user is null)
            throw new InvalidProgramException("Something went wrong");

        var accountInf = _gyMeMapper.GetAccountInfModelDtoMap(user);
        
        return accountInf;
    }

    public async Task<GetAccountInfModel> Update(PutUserDto putUserDto)
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
        
        return _gyMeMapper.GetAccountInfModelDtoMap(user);
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