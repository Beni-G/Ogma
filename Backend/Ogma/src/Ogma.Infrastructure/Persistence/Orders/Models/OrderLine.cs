using Ogma.Infrastructure.Persistence.Orders.ValueObjectRecords;
using Ogma.Infrastructure.Persistence.SharedKernel.BaseTypes;

namespace Ogma.Infrastructure.Persistence.Orders.Models;

public class OrderLine : Entity
{
    public long OrderId { get; set; }
    public Order Order { get; set; } = default!;
    public OrderItem OrderItem { get; set; } = default!;
    public decimal OrderedQuantity { get; set; }
    public decimal CancelledQuantity { get; set; }
    public decimal FullfilledQuantity { get; set; }
    public decimal PriceAmount { get; set; }
    public string PriceCurrency { get; set; } = default!;
    public decimal ExchangeRate { get; set; }
    public string ExchangeTargetCurrency { get; set; } = default!;
    public string? AdditionalInformation { get; set; }
}
