using LiteBus.Messaging.Abstractions;

using Notredame.Domain.Commons;

namespace Notredame.App.Common;

public sealed class EnvironmentLitebus<T> : IEnvironmentExecution<T> where T : class
{
    public T? MessageResult
        => AmbientExecutionContext.Current.MessageResult is T result ? result : null;
}