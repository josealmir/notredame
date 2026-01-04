using Notredame.Domain;
using Notredame.Domain.Repositories;

namespace Notredame.Infra.Data;

public sealed class CepRepository(AppDbContext context)
    : Repository<Cep>(context),  ICepRepository
{
    public async Task AddAsync(Cep cep)
        => await Context.Ceps.AddAsync(cep);
}