namespace Notredame.Domain.Commons;

public abstract class Entity
{
    public long Id { get; set; }
    public Guid ExternalId { get; init; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow; 
    public DateTimeOffset? ModifiedAt { get; set; } = null;
}