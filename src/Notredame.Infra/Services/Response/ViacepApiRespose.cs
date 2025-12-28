using Notredame.Domain;
using Notredame.Domain.DTOs;

namespace Notredame.Infra.Services.Response;

public record ViacepApiRespose
{
    public string Cep { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Ibge { get; set; } = string.Empty;

    public static implicit operator CepDTO?(ViacepApiRespose? response)
        => response is null ? null :  new CepDTO(response.Cep, response.Localidade, response.Bairro, response.Estado, response.Ibge, ProviderCep.Viacep, null);

}