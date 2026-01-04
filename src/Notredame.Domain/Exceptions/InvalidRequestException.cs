
namespace Notredame.Domain.Exceptions;

public class InvalidRequestException : DomainException
{
    public InvalidRequestException(string message) : base(message)
    { }
    public InvalidRequestException(string message, Exception innerException) : base(message, innerException)
    { }
}