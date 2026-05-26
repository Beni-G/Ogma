using Ogma.Domain.Orders.ValueObjects;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Orders.Entities;

public record OrderLineInput(
    long Id,
    OrderItem OrderItem,
    decimal OrderedQuantity,
    decimal CancelledQuantity,
    decimal FullfilledQuantity,
    Money Price,
    ExchangeRate? ExchangeRate = null,
    string? AdditionalInformation = ""
    );
