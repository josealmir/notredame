namespace Notredame.Domain.Exceptions;

public class InvalidCepException : InvalidRequestException
{
    public InvalidCepException(string message) : base(message)
    {
    }

    public InvalidCepException(string message, Exception innerException) : base(message, innerException)
    {
    }
}