using Microsoft.EntityFrameworkCore;
using Notredame.Domain;
using Notredame.Domain.Repositories;

namespace Notredame.Infra.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Cep> Ceps => Set<Cep>();
    public  DbSet<Location> Locations => Set<Location>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}