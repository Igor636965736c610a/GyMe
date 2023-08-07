using System.Security.Claims;
using GymAppInfrastructure.IServices;
using Microsoft.AspNetCore.Http;

namespace GymAppInfrastructure.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    public Guid UserId
    {
        get
        {
            if (User is null)
                throw new InvalidProgramException();
            var first = User.FindFirst(c => c.Type == "id") ?? throw new InvalidProgramException();
            return Guid.Parse(first.Value);
        }
    }
}