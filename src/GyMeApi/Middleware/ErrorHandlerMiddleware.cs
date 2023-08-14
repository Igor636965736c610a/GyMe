using System.Net;
using System.Text.Json;
using GymAppInfrastructure.Exceptions;

namespace GymAppApi.Middleware;

public class ErrorHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger)
    {
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch(error)
            {
                case InvalidOperationException:
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogError("An InvalidOperationException occurred: {ErrorMessage}", error?.Message);

                    break;
                }

                case NullReferenceException:
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    _logger.LogError("A NullReferenceException occurred: {ErrorMessage}", error?.Message);
                    
                    break;
                }

                case ForbiddenException:
                {
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    _logger.LogError("A ForbiddenException occurred: {ErrorMessage}", error?.Message);
                    
                    break;
                }

                case ArgumentException:
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogError("An ArgumentException occurred: {ErrorMessage}", error?.Message);

                    break;
                }

                case InvalidProgramException:
                {
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError("CRITICAL: {ErrorMessage}", error?.Message);

                    break;
                }
                
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError("CRITICAL: {ErrorMessage}", error?.Message);

                    break;
            }

            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}