using GymAppInfrastructure.Dtos.Account;
using GymAppInfrastructure.Dtos.User;

namespace GymAppInfrastructure.IServices;

public interface IAccountService
{
    Task<GetAccountDto> GetInf();
    Task Update(PutUserDto putUserDto);
    Task Remove();
}