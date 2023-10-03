using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace GyMeInfrastructure.Requirements;

public class SourceRequirement : IAuthorizationRequirement
{
    
}

public class SourceRequirementHandler : AuthorizationHandler<SourceRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SourceRequirement requirement)
    {
        var claim = context.User.FindFirst(x => x.Type == "AppSys");
        if (claim is not null)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}