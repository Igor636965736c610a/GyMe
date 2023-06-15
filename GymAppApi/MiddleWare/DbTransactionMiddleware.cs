using GymAppInfrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace GymAppApi.MiddleWare;

public class DbTransactionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly GymAppContext _dbContext;

    public DbTransactionMiddleware(RequestDelegate next, GymAppContext dbContext)
    {
        _next = next;
        _dbContext = dbContext;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            await _next(context);

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}