using LiteBus.Queries.Abstractions;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Notredame.Domain.DTOs;
using Notredame.Shared.Models;
using Notredame.Domain.Repositories;

using OperationResult;

namespace Notredame.App.CepPaginated;

public sealed class QueryCepPaginatedHandler(
    ILogger<QueryCepPaginatedHandler> logger,
    ICepRepository repository, 
    IMapper mapper): 
    IQueryHandler<QueryCepPaginated, Result<PageResult<CepDTO>>>
{
    public async Task<Result<PageResult<CepDTO>>> HandleAsync(
        QueryCepPaginated message, 
        CancellationToken cancellationToken = new CancellationToken())
    {
        using (logger.BeginScope(new Dictionary<string, object> { { "searchBy", message.SearchBy } }))
        {
            var result = await repository.GetAllPaginatedAsync(message.PageNumber, message.PageSize, message.SearchBy);
            var page = new PageResult<CepDTO>(
                 result.Data.Adapt<IEnumerable<CepDTO>>(),
                 result.Page.TotalData, 
                 message.PageNumber,
                 message.PageSize
                ); 
            return Result.Success(page);
        }
    }
}