using Notredame.Domain;
using Notredame.Domain.DTOs;
using Notredame.Domain.Services;

namespace Notredame.Api.Test.MockService;

public class TestCepService : ICepService
{
    private static readonly string[] CepsInvalid = ["00000000", "00000-000", "11111-111"];
    private const string CepTimeOut = "99999999"; 
    public async Task<CepDTO?> SearchCepAsync(string cep)
    {
        
        if (CepsInvalid.Contains(cep))
            return await Task.FromResult<CepDTO?>(null);

        if (CepTimeOut == cep)
            throw new TimeoutException();

        return await Task.FromResult<CepDTO>(new CepDTO{
            Location = new LocationDTO { Lat = -897.2122, Lon = 88779.78787 },
            City = "City DTO",
            State = "State DTO",
            District = "District DTO",
            CreatedAt = DateTimeOffset.UtcNow,
            Provider = ProviderCep.Brazilapi,
            ZipCode = cep,
        });
    }
}