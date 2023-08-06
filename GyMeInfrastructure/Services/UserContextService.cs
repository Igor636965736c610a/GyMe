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
    public Guid GetUserId => User is null ? 
        throw new InvalidProgramException("Something went wrong") : 
        User.FindFirst(c=> c.Type == "id") is null ? 
            throw new InvalidProgramException("Something went wrong") :
            Guid.Parse(User.FindFirst(c=> c.Type == "id")!.Value);
}