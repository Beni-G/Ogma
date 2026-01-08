namespace Ogma.Infrastructure.Persistence.Orders.Models;

public class OrderType
{
    public long Id { get; set; }
    public string Code { get; set; } = default!;
    public string Description { get; set; } = default!;
}
