namespace Ogma.Application.SharedKernel.Dtos;

public record BankAccountDto(string Bank, string Iban, string Currency, string? Bic);
