using GymAppInfrastructure.Context;

namespace GymAppApi.MiddleWare;

public class DbTransactionMiddleware
{
    private readonly RequestDelegate _next;

    public DbTransactionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, GymAppContext context)
    {
        var strategy = context.Database.CreateExecutionStrategy(); 
        await strategy.ExecuteAsync<object, object>(null!, operation: async (dbctx, state, cancel) =>
        {
            // start the transaction
            await using var transaction = await context.Database.BeginTransactionAsync();

            // invoke next middleware 
            await _next(httpContext);

            // commit the transaction
            await transaction.CommitAsync();

            return null!;
        }, null);
    }
}