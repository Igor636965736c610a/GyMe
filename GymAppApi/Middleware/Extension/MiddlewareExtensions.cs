namespace GymAppApi.Middleware.Extension;

public static class MiddlewareExtensions
{
    public static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddScoped<IMiddleware, DbTransactionMiddleware>();
        services.AddScoped<IMiddleware, ErrorHandlerMiddleware>();

        return services;
    }
}