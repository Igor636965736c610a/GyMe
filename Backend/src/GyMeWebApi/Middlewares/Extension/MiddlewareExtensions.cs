namespace GyMeWebApi.Middlewares.Extension;

public static class MiddlewareExtensions
{
    public static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddScoped<DbTransactionMiddleware>();
        services.AddScoped<ErrorHandlerMiddleware>();
        services.AddScoped<ValidAccountMiddleware>();

        return services;
    }
}