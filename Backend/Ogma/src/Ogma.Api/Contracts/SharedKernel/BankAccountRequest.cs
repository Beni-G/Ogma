namespace Ogma.Api.Contracts.SharedKernel;

public record BankAccountRequest(string Bank, string Iban, string Currency, string? Bic);
