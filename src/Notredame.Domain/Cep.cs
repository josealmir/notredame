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
        => new CepCreatedDTO
        {
            ZipCode = ZipCode,
            City = City,
            District = District,
            State = State,
            CreatedAt = CreatedAt,
            Provider = Provider,
            Location = new LocationDTO
            {
                Lon = Location.Lon,
                Lat = Location.Lat,
            },
            ExternalId = ExternalId
        };


}
