namespace Ogma.Api.Contracts.SharedKernel;

public record ExchangeRateRequest(string BaseCurrency, string TargetCurrency, decimal Rate);