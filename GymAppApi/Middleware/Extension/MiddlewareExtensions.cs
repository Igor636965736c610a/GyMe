namespace GymAppApi.Middleware.Extension;

public static class MiddlewareExtensions
{
    public static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddScoped<DbTransactionMiddleware>();
        services.AddScoped<ErrorHandlerMiddleware>();

        return services;
    }
}