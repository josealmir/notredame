using Notredame.Domain.DTOs;
using Notredame.Shared.Infra;
using Notredame.Shared.Models;

namespace Notredame.App.CepPaginated;

public sealed class QueryCepPaginated : IQueryBus<PageResult<CepResult>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchBy { get; set; }
}