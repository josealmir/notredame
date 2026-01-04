using LiteBus.Queries.Abstractions;
using Microsoft.Extensions.Logging;
using Notredame.Domain.DTOs;
using Notredame.Domain.Exceptions;
using Notredame.Domain.Services;
using OperationResult;

using Cep = Notredame.Domain.VOs.Cep;

namespace Notredame.App.GetCep;

public sealed class QueryCepHandler(
    ILogger<QueryCepHandler> logger,
    ICepService cepService):
    IQueryHandler<QueryCep, Result<CepDTO>>
{
    public async Task<Result<CepDTO>> HandleAsync(QueryCep message, CancellationToken cancellationToken = new CancellationToken())
    {
        using (logger.BeginScope(new Dictionary<string, object> { { "cep", message.Cep } }))
        {
            var isValid = Cep.TryParse(message.Cep, out var cep);
            if (!isValid)
                return Result.Error<CepDTO>(new InvalidCepException($"Invalid cep {message.Cep}"));

            var cepDto = await cepService.SearchCepAsync(cep);
            if (CepDTO.IsValid(cepDto))
                return Result.Success<CepDTO>(cepDto);
            
            return Result.Error<CepDTO>(new BusinessNotredameException("Cep Not found"));
        }
    }
}