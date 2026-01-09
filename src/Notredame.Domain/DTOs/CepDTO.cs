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
           :base(zipCode, 
               city, 
               district, 
               state, 
               ibge, 
               provider,
               location)  
            { }
    public static bool IsValid(CepDTO? dto)
        => dto != null && !string.IsNullOrWhiteSpace(dto?.ZipCode);
}