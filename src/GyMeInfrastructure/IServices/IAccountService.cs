using GymAppInfrastructure.Models.Account;
using GymAppInfrastructure.Models.User;

namespace GymAppInfrastructure.IServices;

public interface IAccountService
{
    Task<GetAccountInfModel> GetInf();
    Task SetUserProfile(byte[] image);
    Task Update(PutUserDto putUserDto);
    Task Remove();
}