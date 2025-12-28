namespace Notredame.Domain.Exceptions;

public sealed class BusinessNotredameException : DomainException
{
    public BusinessNotredameException(string message) : base(message) { }
    
    public BusinessNotredameException(string message, Exception inner) : base(message, inner) { }
}