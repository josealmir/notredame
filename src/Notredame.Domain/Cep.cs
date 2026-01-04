using Notredame.Domain.Commons;
using Notredame.Domain.DTOs;

namespace Notredame.Domain;

public class Cep : Entity
{
    public string ZipCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Ibge { get; set; } = string.Empty;
    public Location Location { get; set; } = new();
    public ProviderCep Provider { get; set; }

    public CepCreatedDTO Map()
    {
        var cepCreated = new CepCreatedDTO(
            ZipCode,
            City, 
            District, 
            State, 
            Ibge, 
            Provider, 
            Location.Map());
        cepCreated.Id = ExternalId;
        return cepCreated;
    }
}
