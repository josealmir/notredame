namespace Notredame.Domain.DTOs;

public record CepDTO: CepAbstract
{
    public CepDTO(string zipCode,
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
        CreatedAt = DateTime.UtcNow;
        ExternalId = Guid.NewGuid();
    }

    public CepDTO() { }

    public static bool IsValid(CepDTO? dto)
        => dto != null && !string.IsNullOrWhiteSpace(dto?.ZipCode);

}