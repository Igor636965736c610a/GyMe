using System.Security.Claims;
using GyMeInfrastructure.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;

namespace GyMeInfrastructure.Services;

internal class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public HttpContext HttpContent => _httpContextAccessor.HttpContext ?? throw new InvalidProgramException("HttpContext null");

    public string Email
    {
        get
        {
            if (User is null)
                throw new InvalidProgramException();
            var first = User.FindFirst(c => c.Type == ClaimTypes.Email) ?? throw new InvalidProgramException();
            return first.Value;
        }
    }
    
    public bool EmailConfirmed
    {
        get
        {
            if (User is null)
                throw new InvalidProgramException();
            var first = User.FindFirst(c => c.Type == "EmailConfirmed") ?? throw new InvalidProgramException();
            return bool.Parse(first.Value);
        }
    }

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