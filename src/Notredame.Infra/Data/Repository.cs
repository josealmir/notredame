using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Notredame.Domain.Commons;

namespace Notredame.Infra.Data;

public abstract class Repository<T> where T: Entity
{
    internal readonly AppDbContext Context; 
    internal readonly DbSet<T> DbSet;
    
    internal DbConnection Connection
    {
        get
        {
            var connect = Context.Database.GetDbConnection();
            if (connect.State != ConnectionState.Open)
                connect.OpenAsync();
            return connect;
        }
    }

    internal Repository(AppDbContext context)
    {
        Context = context;
        DbSet = Context.Set<T>();
    }
}