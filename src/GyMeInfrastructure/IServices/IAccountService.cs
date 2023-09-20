using GymAppInfrastructure.Models.Account;
using GymAppInfrastructure.Models.User;
using Microsoft.AspNetCore.Http;

namespace GymAppInfrastructure.IServices;

public interface IAccountService
{
    Task<GetAccountInfModel> GetInf();
    Task SetUserProfile(IFormFile image);
    Task Update(PutUserDto putUserDto);
    Task Remove();
}