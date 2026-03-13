namespace Notredame.Domain.DTOs;

public record CepResult : CepAbstract
{
    public Guid ExternalId { get; set; }
}