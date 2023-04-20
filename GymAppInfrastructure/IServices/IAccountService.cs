using GymAppInfrastructure.Dtos.User;

namespace GymAppInfrastructure.IServices;

public interface IAccountService
{
    Task Update(Guid userId, PutUserDto putUserDto);
}