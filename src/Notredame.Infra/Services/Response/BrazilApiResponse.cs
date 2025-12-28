using Notredame.Domain;
using Notredame.Domain.DTOs;

namespace Notredame.Infra.Services.Response;

public record BrazilApiResponse
{
    public string Cep { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    
    public LocationResponse Location { get; set; } = new(); 

    public static implicit operator CepDTO?(BrazilApiResponse? response)
        => response is null ? null : new CepDTO(response.Cep, response.City, response.Neighborhood, response.State, string.Empty,
            ProviderCep.Brazilapi,  new LocationDTO
            {
                Lat = response?.Location?.Coordinates?.Latitude ?? 0,
                Lon = response?.Location?.Coordinates?.Longitude ?? 0,    
            });
}

