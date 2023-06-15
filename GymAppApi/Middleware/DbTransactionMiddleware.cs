using GymAppInfrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace GymAppApi.Middleware;

public class DbTransactionMiddleware : IMiddleware
{
    private readonly GymAppContext _dbContext;

    public DbTransactionMiddleware(GymAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            await next(context);

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw; //mój exception se zrobie jakiś 
        }
    }
}