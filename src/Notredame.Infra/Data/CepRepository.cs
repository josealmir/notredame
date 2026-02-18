using MapsterMapper;
using Notredame.Domain;
using Microsoft.EntityFrameworkCore;
using Notredame.Domain.Repositories;
using Notredame.Shared.Models;

namespace Notredame.Infra.Data;

public sealed class CepRepository(AppDbContext context, IMapper mapper)
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
    
    public async Task<Cep> GetById(Guid externalId)
        => await Context.Ceps.SingleAsync(x => x.ExternalId.Equals(externalId));
    
    public async Task<PageResult<Cep>> GetAllPaginatedAsync(
        int pageNumber, 
        int pageSize, 
        string? searchBy)

    {
        var hasFilter = !string.IsNullOrEmpty(searchBy);
        var query = Context.Set<Cep>().AsQueryable();
        if (hasFilter)
            query  = query.Where(x => x.ZipCode.Contains(searchBy));
        
        var result  = await query
            .Include(x=>x.Location)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .OrderByDescending(x=> x.Id)
            .TagWith("Get_All_Paginated_Ceps_Async")
            .ToListAsync();
        
        var totalData = hasFilter ? 
            await Context.Ceps.CountAsync(c => c.ZipCode.Contains(searchBy)) :
            await Context.Ceps.CountAsync();
        
        return  new PageResult<Cep>(result, totalData, pageNumber, pageSize); 
    }
}