using GyMeApplication.Models.Account;
using GyMeApplication.Models.User;
using Microsoft.AspNetCore.Http;

namespace GyMeApplication.IServices;

public interface IAccountService
{
    Task<GetAccountInfModel> GetInf();
    Task SetUserProfile(IFormFile image);
    Task Update(PutUserDto putUserDto);
    Task Remove();
}