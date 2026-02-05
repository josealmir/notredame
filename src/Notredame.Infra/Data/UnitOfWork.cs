using Microsoft.Extensions.Logging;
using Notredame.Domain.Repositories;

namespace Notredame.Infra.Data;

public class UnitOfWork(AppDbContext context, ILogger<UnitOfWork> logger) : IUnitOfWork
{
    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
    {
        var result = await context.SaveChangesAsync(cancellationToken) > 0;
        logger.LogInformation("Operacao efetivada com sucesso: {0}", result);
        return result;
    }
}