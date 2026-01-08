namespace Ogma.Infrastructure.Persistence.Orders.Models;

public class Order
{
    public long Id { get; set; }
    public long PartnerId { get; set; }
    public string PartnerName { get; set; } = default!;
    public string OrderNumber { get; set; } = default!;
    public DateTime OrderDate { get; set; }
    public long OrderTypeId { get; set; }
    public OrderType OrderType { get; set; } = default!;
    public long OrderStatusId { get; set; }
    public OrderStatus OrderStatus { get; set; } = default!;
    public string? AdditionalInformation { get; set; }
    public List<OrderLine> OrderLines { get; set; } = new();
}
