using System.Transactions;
using GyMeApplication.Options;
using GyMeApplication.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GyMeWebApi.Middlewares;

public class DbTransactionMiddleware : IMiddleware
{
    private readonly GyMePostgresContext _dbContext;

    public DbTransactionMiddleware(GyMePostgresContext dbContext)
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
        catch (Exception)
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