using Ogma.Infrastructure.Persistence.SharedKernel.BaseTypes;

namespace Ogma.Infrastructure.Persistence.Orders.Models;

public class OrderStatus : Entity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}
