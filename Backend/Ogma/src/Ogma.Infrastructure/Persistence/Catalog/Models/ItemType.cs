using Ogma.Infrastructure.Persistence.SharedKernel.BaseTypes;

namespace Ogma.Infrastructure.Persistence.Catalog.Models;
public class ItemType : Entity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}
