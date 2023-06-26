using GymAppInfrastructure.Dtos.Account;
using GymAppInfrastructure.Dtos.User;

namespace GymAppInfrastructure.IServices;

public interface IAccountService
{
    Task<GetAccountDto> GetInf(Guid userId);
    Task Update(Guid userId, PutUserDto putUserDto);
    Task Remove(Guid userId);
}