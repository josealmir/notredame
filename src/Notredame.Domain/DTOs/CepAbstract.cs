namespace Notredame.Domain.DTOs;

public abstract record CepAbstract
{
    protected CepAbstract(string zipCode, 
        string city, 
        string district, 
        string state, 
        string ibge, 
        ProviderCep provider, 
        LocationDTO? location)
    {
        ZipCode = zipCode;
        City = city;
        District = district;
        State = state;
        Ibge = ibge;
        Provider = provider;
        Location = location;
    }

    public string ZipCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty; 
    public string District { get; set; } = string.Empty; 
    public string State { get; set; } = string.Empty; 
    public string Ibge { get; set; } = string.Empty; 
    public ProviderCep Provider { get; set; } 
    public LocationDTO Location { get; set; } = new LocationDTO();
}