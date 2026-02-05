namespace Notredame.Domain.Repositories;

public interface ICepRepository
{
    /// <summary>
    /// Save new cep in database
    /// </summary>
    /// <param name="cep"></param>
    /// <returns></returns>
    public Task AddAsync(Cep cep);

    /// <summary>
    /// Return cep by externalId.
    /// </summary>
    /// <param name="externalId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<Cep?> GerByIdAsync(Guid externalId, CancellationToken cancellationToken);

    /// <summary>
    /// Delete cep with entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public void Delete(Cep entity);
}