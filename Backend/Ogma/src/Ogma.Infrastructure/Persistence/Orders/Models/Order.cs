using Ogma.Infrastructure.Persistence.Orders.ValueObjectRecords;
using Ogma.Infrastructure.Persistence.SharedKernel.BaseTypes;

namespace Ogma.Infrastructure.Persistence.Orders.Models;

public class Order : Entity
{
    public OrderPartner OrderPartner { get; set; } = default!;
    public string OrderNumber { get; set; } = default!;
    public DateTime OrderDate { get; set; }
    public long OrderTypeId { get; set; }
    public OrderType OrderType { get; set; } = default!;
    public long OrderStatusId { get; set; }
    public OrderStatus OrderStatus { get; set; } = default!;
    public string? AdditionalInformation { get; set; }
    public List<OrderLine> OrderLines { get; set; } = new();
}
