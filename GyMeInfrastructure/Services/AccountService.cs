using AutoMapper;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Account;
using GymAppInfrastructure.Dtos.User;
using GymAppInfrastructure.Exceptions;
using GymAppInfrastructure.IServices;

namespace GymAppInfrastructure.Services;

internal class AccountService : IAccountService
{
    private readonly IUserRepo _userRepo;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    
    public AccountService(IUserRepo userRepo, IMapper mapper, IUserContextService userContextService)
    {
        _userRepo = userRepo;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<GetAccountDto> GetInf()
    {
        var userIdFromJwt = _userContextService.GetUserId;
        
        var user = await _userRepo.Get(userIdFromJwt);
        if (user is null)
            throw new InvalidOperationException("Something went wrong");

        var userDto = _mapper.Map<User, GetAccountDto>(user);

        return userDto;
    }

    public async Task Update(PutUserDto putUserDto)
    {
        var userIdFromJwt = _userContextService.GetUserId;
        
        var user = await _userRepo.Get(userIdFromJwt);
        if (user is null)
            throw new InvalidOperationException("Something went wrong");
        
        user.UserName = putUserDto.UserName;
        user.FirstName = putUserDto.FirstName;
        user.LastName = putUserDto.LastName;
        user.PrivateAccount = putUserDto.PrivateAccount;
        
        await _userRepo.Update(user);
    }

    public async Task Remove()
    {
        var userIdFromJwt = _userContextService.GetUserId;
        
        var user = await _userRepo.Get(userIdFromJwt);
        if (user is null)
            throw new InvalidOperationException("Something went wrong");

        await _userRepo.RemoveUser(user);
    }
}