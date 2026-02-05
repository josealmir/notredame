using FluentValidation;
using LiteBus.Commands.Abstractions;
using MapsterMapper;
using OperationResult;
using Notredame.Domain.Services;
using Notredame.Domain.Exceptions;
using Microsoft.Extensions.Logging;

using Notredame.Domain;
using Notredame.Domain.Commons;
using Notredame.Domain.DTOs;
using Notredame.Domain.Repositories;

namespace Notredame.App.CreateCep;

public sealed class CepCommandHandler(
        ICepService cepService,
        IMapper mapper,
        ICepRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<CepCommandHandler> logger)
        : ICommandHandler<CepCommand, Result<(string, object)>>
{
    private const string UriCreated = "/api/v1/ceps/"; 
        
    public async Task<Result<(string, object)>> HandleAsync(CepCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        using (logger.BeginScope(new Dictionary<string, object> { { "zipCode", message.ZipCode } }))
        {
            ArgumentNullException.ThrowIfNull(message);
            
            var cep = await cepService.SearchCepAsync(message.ZipCode);
            if (!CepDTO.IsValid(cep))
                return Result.Error<(string, object)>(new BusinessNotredameException("Not found cep")); 

            var entity = mapper.Map<Domain.Cep>(cep);
            entity.Location = new Location { CepId = entity.Id, Lat = entity.Location.Lat, Lon = entity.Location.Lon };
            
            await repository.AddAsync(entity);
            await unitOfWork.CommitAsync(cancellationToken);
            return Result.Success<(string, object)>(($"{UriCreated}{entity.ExternalId}", entity.Map()));
        }
    }
}