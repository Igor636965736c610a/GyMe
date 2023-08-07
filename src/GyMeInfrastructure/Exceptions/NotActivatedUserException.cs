namespace GymAppInfrastructure.Exceptions;

public class NotActivatedUserException : Exception
{
    public NotActivatedUserException()
    {
    }
    public NotActivatedUserException(string message) : base(message)
    {
    }
    public NotActivatedUserException(string message, Exception inner)
        : base(message, inner)
    {
    }
}