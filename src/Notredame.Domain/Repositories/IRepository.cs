using Notredame.Shared.Models;

namespace Notredame.Domain.Repositories;

public interface IRepository<T> where T : class
{
    Task<T> GetById(Guid externalId);
    
    Task<PageResult<T>> GetAllPaginatedAsync(int pageNumber, int pageSize, string? searchBy);
}