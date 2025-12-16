namespace Ogma.Api.Contracts.SharedKernel;

public record BankAccountResponse(string Bank, string Iban, string Currency, string? Bic);