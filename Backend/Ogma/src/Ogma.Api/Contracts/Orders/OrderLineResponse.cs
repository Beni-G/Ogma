using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Orders;

public record OrderLineResponse(
    long Id,
    OrderItemResponse OrderItem,
    decimal OrderedQuantity,
    decimal CancelledQuantity,
    decimal FullfilledQuantity,
    MoneyResponse Price,
    ExchangeRateResponse? ExchangeRate,
    string? AdditionalInformation);
