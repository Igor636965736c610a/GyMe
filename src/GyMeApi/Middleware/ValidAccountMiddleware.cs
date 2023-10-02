using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using GymAppApi.Controllers.HelperAttributes;
using GymAppApi.Routes.v1;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;

namespace GymAppApi.Middleware;

public class ValidAccountMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User.Identity is null || !context.User.Identity.IsAuthenticated)
        {
            await next(context);
            return;
        }

        var valid = context.User.FindFirst("validAccount");
        
        if (valid is null)
        {
            await next(context);
            return;
        }
        
        if (valid.Value == "False")
        {
            var endpoint = context.GetEndpoint();
            var skipValidAccountCheckAttribute = endpoint?.Metadata.GetMetadata<SkipValidAccountCheckAttribute>();
            if (skipValidAccountCheckAttribute != null)
            {
                await next(context);
                return;
            }
            
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = 403;
            var result = JsonSerializer.Serialize(new
            {
                message = "Required account details are not filled",
            });
            await response.WriteAsync(result);
        }

        await next(context);
    }
}