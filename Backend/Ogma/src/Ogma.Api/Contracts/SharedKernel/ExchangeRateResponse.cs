namespace Ogma.Api.Contracts.SharedKernel;

public record ExchangeRateResponse(string BaseCurrency, string TargetCurrency, decimal Rate);
