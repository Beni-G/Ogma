using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Infrastructure.Persistence.SharedKernel.BaseTypes;

namespace Ogma.Infrastructure.Persistence.SharedKernel.Extensions;

public static class BaseMappingExtensions
{
    public static void MapBaseProperties<TEntity>(this TEntity source, Entity destination) where TEntity : Entity<long>
    {
        destination.Id = source.Id;
        destination.CreatedAt = source.Metadata.CreatedAt;
        destination.UpdatedAt = source.Metadata.UpdatedAt;
        destination.Version = source.Metadata.Version;
    }
}
