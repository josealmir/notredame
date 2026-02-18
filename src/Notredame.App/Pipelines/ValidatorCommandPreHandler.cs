using FluentValidation;
using LiteBus.Commands.Abstractions;
using LiteBus.Messaging.Abstractions;
using LiteBus.Messaging.Extensions;
using Microsoft.Extensions.Logging;

namespace Notredame.App.Pipelines;

public sealed class ValidatorCommandPreHandler(
    ILogger<ValidatorCommandPreHandler> logger,
    IServiceProvider serviceProvider) : 
    ICommandValidator<ICommand>
{
    public async Task ValidateAsync(ICommand request, CancellationToken cancellationToken = new CancellationToken())
    {
        using (logger.BeginScope(new Dictionary<string, object> {{"Command", request}}))
        {
            logger.LogInformation("PreHandleAsync {Command}", request.GetType().Name);
            
            var validatorType = typeof(IValidator<>).MakeGenericType(request.GetType());
            var validator = (IValidator)serviceProvider.GetRequiredService(validatorType);
            var validatorContext = new ValidationContext<object>(request);
            
            var result = await validator.ValidateAsync(validatorContext, cancellationToken); 
            if (!result.IsValid)
            {
                logger.LogWarning("Validation failed for {Command} is {result}", request.GetType().Name, result.IsValid);
                var response = CommandBusExtension.CreateErrorResult(request.GetTypeResult(), new ValidationException(result.Errors)); 
                AmbientExecutionContext.Current.Abort(response);
            }
            
            logger.LogWarning("Validation Success for {Command} is {result}", request.GetType().Name, result.IsValid);
        }
    }
}
