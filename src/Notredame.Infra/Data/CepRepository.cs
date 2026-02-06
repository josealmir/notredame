using Microsoft.EntityFrameworkCore;
using Notredame.Domain;
using Notredame.Domain.Repositories;

namespace Notredame.Infra.Data;

public sealed class CepRepository(AppDbContext context)
    : Repository<Cep>(context),  ICepRepository
{
    public async Task AddAsync(Cep cep)
        => await Context.Ceps.AddAsync(cep);

    public async Task<Cep?> GerByIdAsync(Guid id, CancellationToken cancellationToken)
        => await Context
                    .Ceps
                    .Include(c => c.Location)
                    .SingleOrDefaultAsync(x => x.ExternalId == id, cancellationToken);

    public void Delete(Cep entity)
        => Context.Ceps.Remove(entity);
}