using Microsoft.EntityFrameworkCore;
using Notredame.Domain;
using Notredame.Domain.Repositories;

namespace Notredame.Infra.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Cep> Ceps => Set<Cep>();

    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
        => await SaveChangesAsync(cancellationToken) > 0;
}