
namespace Notredame.Domain.Exceptions;

public abstract class InvalidRequestException : DomainException
{
    public InvalidRequestException(string message) : base(message)
    { }
    public InvalidRequestException(string message, Exception innerException) : base(message, innerException)
    { }
}