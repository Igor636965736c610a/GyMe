using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace GymAppInfrastructure.Requirements;

public class SourceRequirement : IAuthorizationRequirement
{
    
}

public class SourceRequirementHandler : AuthorizationHandler<SourceRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SourceRequirement requirement)
    {
        var claim = context.User.FindFirst(x => x.Type == "source");
        if (claim is not null)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}