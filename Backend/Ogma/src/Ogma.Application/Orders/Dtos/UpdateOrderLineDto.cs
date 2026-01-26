using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Application.Orders.Dtos;

public record UpdateOrderLineDto(
    long Id,
    long ItemId,
    decimal OrderedQuantity,
    decimal CancelledQuantity,
    decimal FullfilledQuantity,
    MoneyDto Price,
    ExchangeRateDto? ExchangeRate,
    string? AdditionalInformation
);
