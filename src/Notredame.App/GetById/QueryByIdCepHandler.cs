using LiteBus.Queries.Abstractions;

using Mapster;

using Notredame.Domain.DTOs;
using Notredame.Domain.Exceptions;
using Notredame.Domain.Repositories;
using MapsterMapper;
using OperationResult;
using Microsoft.Extensions.Logging;

namespace Notredame.App.GetById;

public sealed class QueryByIdCepHandler(
    ILogger<QueryByIdCepHandler> logger,
    ICepRepository repository, 
    IMapper mapper): 
    IQueryHandler<QueryByIdCep, Result<CepDTO>>
{
    public async Task<Result<CepDTO>> HandleAsync(
        QueryByIdCep message,
        CancellationToken cancellationToken = new CancellationToken())
    {
        using (logger.BeginScope(new Dictionary<string, object> { { "externalId", message.ExternalId } }))
        {
            var cep = await repository.GerByIdAsync(message.ExternalId, cancellationToken);
            return cep is null ? Result.Error<CepDTO>(new BusinessNotredameException("Cep not found")) : Result.Success(mapper.Map<CepDTO>(cep));   
        }
    }
}