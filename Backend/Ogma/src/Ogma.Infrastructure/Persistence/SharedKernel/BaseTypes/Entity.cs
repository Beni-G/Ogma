namespace Ogma.Infrastructure.Persistence.SharedKernel.BaseTypes;

public class Entity
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Version { get; set; }
}
