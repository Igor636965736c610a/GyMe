using System.Security.Claims;

namespace GymAppInfrastructure.IServices;

public interface IUserContextService
{
    ClaimsPrincipal? User { get; }
    string Email { get; }
    Guid UserId { get; }
}