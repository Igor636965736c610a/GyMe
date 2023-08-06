namespace GymAppInfrastructure.Exceptions;

public class SaveChangesDbException : Exception
{
    public SaveChangesDbException()
    {
    }
    public SaveChangesDbException(string message) : base(message)
    {
    }
    public SaveChangesDbException(string message, Exception inner)
        : base(message, inner)
    {
    }
}