using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Application.Orders.Dtos;

public record CreateOrderLineDto(
    long ItemId,
    decimal OrderedQuantity,
    MoneyDto Price,
    ExchangeRateDto? ExchangeRate,
    string? AdditionalInformation);
