using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Application.Orders.Dtos;

public record OrderLineDto(
    long Id,
    OrderItemDto OrderItem,
    decimal OrderedQuantity,
    decimal CancelledQuantity,
    decimal FullfilledQuantity,
    MoneyDto Price,
    ExchangeRateDto? ExchangeRate,
    string? AdditionalInformation
);