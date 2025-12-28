using Notredame.Domain.DTOs;

namespace Notredame.Domain.Services;

public interface ICepService
{
    public Task<CepDTO?> SearchCepAsync(string cep);
}