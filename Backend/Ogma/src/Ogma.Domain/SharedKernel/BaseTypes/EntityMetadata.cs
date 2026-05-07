namespace Ogma.Domain.SharedKernel.BaseTypes;

public record EntityMetadata(DateTime CreatedAt, DateTime UpdatedAt, int Version);
