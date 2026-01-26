using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Orders;

public record UpdateOrderLineRequest(
    long Id,
    long ItemId,
    decimal OrderedQuantity,
    decimal CancelledQuantity,
    decimal FullfilledQuantity,
    MoneyRequest Price,
    ExchangeRateRequest? ExchangeRate,
    string? AdditionalInformation
    );
