using GyMeInfrastructure.Models.Account;
using GyMeInfrastructure.Models.User;
using Microsoft.AspNetCore.Http;

namespace GyMeInfrastructure.IServices;

public interface IAccountService
{
    Task<GetAccountInfModel> GetInf();
    Task SetUserProfile(IFormFile image);
    Task Update(PutUserDto putUserDto);
    Task Remove();
}