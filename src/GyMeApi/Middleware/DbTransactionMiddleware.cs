using GymAppInfrastructure.Context;
using GymAppInfrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
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
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
        try
        {
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new DbCommitException("DbTransaction", ex);
        }
    }
}