namespace Notredame.Domain.DTOs;

public record CepDTO(string ZipCode, string City, string District, string State, string Ibge, ProviderCep Provider, LocationDTO? Location)
{
    public static bool IsValid(CepDTO? dto)
        => dto != null && !string.IsNullOrWhiteSpace(dto.ZipCode);
}