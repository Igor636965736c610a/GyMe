using GymAppInfrastructure.Dtos.Account;
using GymAppInfrastructure.Dtos.User;

namespace GymAppInfrastructure.IServices;

public interface IAccountService
{
    Task<GetAccountDto> GetAccountInf(Guid userId);
    Task Update(Guid userId, PutUserDto putUserDto);
}