namespace Ogma.Application.SharedKernel.Dtos;

public record ExchangeRateDto(string BaseCurrency, string TargetCurrency, decimal Rate);
