using GymAppInfrastructure.Dtos.Account;
using GymAppInfrastructure.Dtos.User;

namespace GymAppInfrastructure.IServices;

public interface IAccountService
{
    Task<GetAccountDto> GetInf(Guid jwtId);
    Task Update(Guid jwtId, PutUserDto putUserDto);
    Task Remove(Guid jwtId);
}