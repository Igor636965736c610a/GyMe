using System.Net;
using System.Text.Json;
using GymAppInfrastructure.Exceptions;
using GymAppInfrastructure.Models.InternalManagement;
using GymAppInfrastructure.Services.InternalManagement;

namespace GymAppApi.Middleware;

public class ErrorHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly ErrorService _errorService;

    public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger, ErrorService errorService)
    {
        _logger = logger;
        _errorService = errorService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string errorMessage = "CRITICAL";
        try
        {
            await next(context);
        }
        catch (Exception error)
        {
            _logger.LogError("error {ErrorMessage}" , error.ToString());
            var response = context.Response;
            response.ContentType = "application/json";

            switch(error)
            {
                case InvalidOperationException:
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogError("An InvalidOperationException occurred: {ErrorMessage}", error.Message);
                    errorMessage = error.Message;
                    var errorEntity = CreateError(error, response.StatusCode);
                    await _errorService.Add(errorEntity);

                    break;
                }

                case NullReferenceException:
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    _logger.LogError("A NullReferenceException occurred: {ErrorMessage}", error.Message);
                    errorMessage = error.Message;
                    
                    break;
                }

                case ForbiddenException:
                {
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    _logger.LogError("A ForbiddenException occurred: {ErrorMessage}", error.Message);
                    errorMessage = error.Message;
                    
                    break;
                }

                case ArgumentException:
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogError("An ArgumentException occurred: {ErrorMessage}", error.Message);
                    errorMessage = error.Message;

                    break;
                }

                case InvalidProgramException:
                {
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError("CRITICAL: {ErrorMessage}", error.Message);
                    var errorEntity = CreateError(error, response.StatusCode);
                    await _errorService.Add(errorEntity);

                    break;
                }

                case DbCommitException:
                {
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError("CRITICAL: {ErrorMessage}", error.Message);
                    var errorEntity = CreateError(error, response.StatusCode);
                    await _errorService.Add(errorEntity);

                    break;
                }

                default:
                {
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError("CRITICAL: {ErrorMessage}", error.Message);
                    var errorEntity = CreateError(error, response.StatusCode);
                    await _errorService.Add(errorEntity);

                    break;
                }
            }

            var result = JsonSerializer.Serialize(new { message = errorMessage });
            await response.WriteAsync(result);
        }
    }

    private Error CreateError(Exception exception, int statusCode)
        => new()
        {
            StatusCode = (int)HttpStatusCode.BadRequest,
            StackStrace = exception.StackTrace,
            Message = exception.Message
        };
}