using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GymAppInfrastructure.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class BlockCorsAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (IsCorsRequest(context))
        {
            context.Result = new BadRequestResult();
            return;
        }

        base.OnActionExecuting(context);
    }

    private bool IsCorsRequest(ActionExecutingContext context)
    {
        return context.HttpContext.Request.Headers.ContainsKey("Origin");
    }
}
