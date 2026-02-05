using System.Reflection;
using FluentValidation;
using LiteBus.Commands.Abstractions;
using Notredame.Shared.Infra;
using OperationResult;

namespace Notredame.App.Pipelines;

public static class CommandBusExtension
{
    extension(ICommand command)
    {
        public Type GetTypeResult()
        {
            return command.GetType()
                .GetInterfaces()
                .First(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(ICommandBus<>))
                .GetGenericArguments()[0];
        }
    }
    
    public static object CreateErrorResult(
        Type payloadType,
        ValidationException exception)
    {
        var errorMethod = typeof(Result)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .First(m =>
                m is { Name: "Error", IsGenericMethod: true } &&
                m.GetParameters().Length == 1);

        return errorMethod
            .MakeGenericMethod(payloadType)
            .Invoke(null, [exception])!;
    }
}
