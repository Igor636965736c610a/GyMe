namespace GymAppInfrastructure.Exceptions;

public class DbCommitException : Exception
{
    public DbCommitException()
    {
    }
    public DbCommitException(string message) : base(message)
    {
    }
    public DbCommitException(string message, Exception inner)
        : base(message, inner)
    {
    }
}