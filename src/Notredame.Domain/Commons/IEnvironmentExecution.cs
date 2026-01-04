namespace Notredame.Domain.Commons;

public interface IEnvironmentExecution<out T>
{
    public T? MessageResult { get; }
}