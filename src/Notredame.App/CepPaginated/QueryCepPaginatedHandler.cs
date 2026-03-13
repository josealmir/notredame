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
    IQueryHandler<QueryCepPaginated, Result<PageResult<CepResult>>>
{
    public async Task<Result<PageResult<CepResult>>> HandleAsync(
        QueryCepPaginated message, 
        CancellationToken cancellationToken = new CancellationToken())
    {
        using (logger.BeginScope(new Dictionary<string, object> { { "searchBy", message.SearchBy } }))
        {
            var result = await repository.GetAllPaginatedAsync(message.PageNumber, message.PageSize, message.SearchBy);
            var page = new PageResult<CepResult>(
                 result.Data.Adapt<IEnumerable<CepResult>>() ??  Enumerable.Empty<CepResult>(),
                 result.Page.TotalData, 
                 message.PageNumber,
                 message.PageSize
                ); 
            return Result.Success(page);
        }
    }
}