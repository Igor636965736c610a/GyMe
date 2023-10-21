using GyMeApplication.Models.Account;
using GyMeApplication.Models.User;
using Microsoft.AspNetCore.Http;

namespace GyMeApplication.IServices;

public interface IAccountService
{
    Task<GetAccountInfModel> GetInf();
    Task SetUserProfile(IFormFile image);
    Task<GetAccountInfModel> Update(PutUserDto putUserDto);
    Task Remove();
}