using GymAppCore.IRepo;
using GymAppInfrastructure.Dtos.User;
using GymAppInfrastructure.Exceptions;
using GymAppInfrastructure.IServices;

namespace GymAppInfrastructure.Services;

public class AccountService : IAccountService
{
    private readonly IUserRepo _userRepo;
    public AccountService(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }
    
    public async Task Update(Guid userId, PutUserDto putUserDto)
    {
        var user = await _userRepo.Get(userId);
        if (user is null)
            throw new InvalidOperationException("Something went wrong");
        if (user.Id != userId)
            throw new ForbiddenException("Can't update other user");
        
        user.UserName = putUserDto.UserName;
        user.FirstName = putUserDto.FirstName;
        user.LastName = putUserDto.LastName;
        user.PrivateAccount = putUserDto.PrivateAccount;
        var result = await _userRepo.Update(user);
        if (!result)
            throw new SaveChangesDbException("something went wrong while saving database changes");
    }
}