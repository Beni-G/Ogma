using Ogma.Infrastructure.Persistence.SharedKernel.BaseTypes;

namespace Ogma.Infrastructure.Persistence.Orders.Models;

public class OrderType : Entity
{
    public string Code { get; set; } = default!;
    public string Description { get; set; } = default!;
}
