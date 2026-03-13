namespace Notredame.Domain.DTOs;

public abstract record CepAbstract
{
    public string ZipCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty; 
    public string District { get; set; } = string.Empty; 
    public string State { get; set; } = string.Empty; 
    public string Ibge { get; set; } = string.Empty; 
    public DateTimeOffset CreatedAt { get; set; } 
    public ProviderCep Provider { get; set; } 
    public LocationDTO Location { get; set; } = new LocationDTO();
}