using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Orders;

public record CreateOrderLineRequest(
    long ItemId,
    decimal OrderedQuantity,
    MoneyRequest Price,
    ExchangeRateRequest? ExchangeRate,
    string? AdditionalInformation
    );
