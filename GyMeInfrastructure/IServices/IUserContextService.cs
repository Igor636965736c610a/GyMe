using System.Security.Claims;

namespace GymAppInfrastructure.IServices;

public interface IUserContextService
{
    ClaimsPrincipal? User { get; }
    Guid GetUserId { get; }
}