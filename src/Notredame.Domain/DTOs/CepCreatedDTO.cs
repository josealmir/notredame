namespace Notredame.Domain.DTOs;

public record CepCreatedDTO : CepAbstract
{
    public CepCreatedDTO(string zipCode,
        string city,
        string district,
        string state,
        string ibge,
        ProviderCep provider,
        LocationDTO? location)
        :base(zipCode, 
            city, 
            district, 
            state, 
            ibge, 
            provider,
            location)  
    { }
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}