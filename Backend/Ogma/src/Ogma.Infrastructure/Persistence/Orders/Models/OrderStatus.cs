namespace Ogma.Infrastructure.Persistence.Orders.Models;

public class OrderStatus
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}
