using Microsoft.EntityFrameworkCore;
using Notredame.Domain.Commons;

namespace Notredame.Infra.Data;

public abstract class Repository<T> where T: Entity
{
    internal readonly AppDbContext Context; 
    internal readonly DbSet<T> DbSet;
    
    internal Repository(AppDbContext context)
    {
        Context = context;
        DbSet = Context.Set<T>();
    }
}