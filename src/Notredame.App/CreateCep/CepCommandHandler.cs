using FluentValidation;
using LiteBus.Commands.Abstractions;
using MapsterMapper;
using OperationResult;
using Notredame.Domain.Services;
using Notredame.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Notredame.Domain.Commons;
using Notredame.Domain.Repositories;

namespace Notredame.App.CreateCep;

public sealed class CepCommandHandler(
        ICepService cepService,
        IMapper mapper,
        ICepRepository repository,
        IUnitOfWork unitOfWork,
        IEnvironmentExecution<Exception> environment,
        ILogger<CepCommandHandler> logger)
        : ICommandHandler<CepCommand, Result<(string, object)>>
{
    private const string UriCreated = "/api/v1/ceps/"; 
        
    public async Task<Result<(string, object)>> HandleAsync(CepCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        using (logger.BeginScope(new Dictionary<string, object> { { "zipCode", message.ZipCode } }))
        {
            if (environment?.MessageResult is ValidationException exception)
                return  Result.Error<(string, object)>(exception);

            var cep = await cepService.SearchCepAsync(message.ZipCode);
            if (cep is null)
                return Result.Error<(string, object)>(new BusinessNotredameException("Not found cep")); 

            var entity = mapper.Map<Domain.Cep>(cep);
            
            await repository.AddAsync(entity);
            await unitOfWork.CommitAsync(cancellationToken);
            return Result.Success<(string, object)>(($"{UriCreated}{entity.ExternalId}", entity.Map()));
        }
    }
}