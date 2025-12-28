using Notredame.Domain.Commons;

namespace Notredame.Domain;

public class Cep : Entity
{
    public string ZipCode { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string District { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string Ibge { get; init; } = string.Empty;
    public Location Location { get; init; } = new();
    public ProviderCep Provider { get; set; }
}
