using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Notredame.Domain.DTOs;
using Notredame.Domain.Services;
using Notredame.Infra.Services.Response;
using Notredame.Shared.Options;

namespace Notredame.Infra.Services;

public sealed class CepService(
    ILogger<CepService> logger, 
    IHttpClientFactory httpClientFactory) : ICepService
{
    public async Task<CepDTO?> SearchCepAsync(string cep)
    {
        using (logger.BeginScope(new Dictionary<string, object> { { "cep", cep } }))
        {
            try
            {
                return await ApiBrazilAsync(cep);
            }
            catch(HttpRequestException ex)
            {
                logger.LogError(ex.Message, ex);
                return await ApiViaCepAsync(cep);
            }   
        }
    }

    private async Task<ViacepApiRespose?> ApiViaCepAsync(string cep, CancellationToken cancellationToken = default)
    {
        var httpClient = httpClientFactory.CreateClient(ViaCepOptionNotredame.SectionName);
        var response = await httpClient.GetFromJsonAsync<ViacepApiRespose>($"ws/{cep}/json", cancellationToken);
        return response;
    }

    private async Task<BrazilApiResponse?> ApiBrazilAsync(string cep, CancellationToken cancellationToken = default)
    {
        var httpClient = httpClientFactory.CreateClient(BrasilCepOptionNotredame.SectionName);
        var response = await httpClient.GetFromJsonAsync<BrazilApiResponse>($"api/cep/v2/{cep}", cancellationToken);
        
        return response;
    }
}