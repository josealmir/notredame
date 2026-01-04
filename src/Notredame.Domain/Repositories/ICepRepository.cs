namespace Notredame.Domain.Repositories;

public interface ICepRepository
{
    /// <summary>
    /// Save new cep in database
    /// </summary>
    /// <param name="cep"></param>
    /// <returns></returns>
    public Task AddAsync(Cep cep);
}