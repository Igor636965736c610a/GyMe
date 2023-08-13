using AutoMapper;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Models.Account;
using GymAppInfrastructure.Models.User;
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

    public async Task<GetAccountInfModel> GetInf()
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var user = await _userRepo.GetOnlyValid(userIdFromJwt);
        if (user is null)
            throw new InvalidOperationException("Something went wrong");

        var accountInf = _mapper.Map<User, GetAccountInfModel>(user);
        
        if (user.ExtendedUser is not null)
        {
            accountInf.Gender = (GenderDto)user.ExtendedUser.Gender;
            accountInf.ProfilePicture = user.ExtendedUser.ProfilePicture;
        }
        
        return accountInf;
    }

    public async Task Update(PutUserDto putUserDto)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var user = await _userRepo.GetOnlyValid(userIdFromJwt);
        if (user?.ExtendedUser is null)
            throw new InvalidOperationException("Something went wrong");
        
        user.UserName = putUserDto.UserName;
        user.FirstName = putUserDto.FirstName;
        user.LastName = putUserDto.LastName;
        user.ExtendedUser.PrivateAccount = putUserDto.PrivateAccount;
        
        await _userRepo.Update(user);
    }
    
    public async Task SetUserProfile(byte[] image)
    {
        var userIdFromJwt = _userContextService.UserId;

        var user = await _userRepo.GetOnlyValid(userIdFromJwt);

        if (user?.ExtendedUser is null)
            throw new InvalidProgramException("Something went wrong");

        user.ExtendedUser.ProfilePicture = image;

        await _userRepo.Update(user);
    }

    public async Task Remove()
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var user = await _userRepo.GetOnlyValid(userIdFromJwt);
        if (user is null)
            throw new InvalidOperationException("Something went wrong");

        await _userRepo.RemoveUser(user);
    }
}