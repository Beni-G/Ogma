using System.ComponentModel.DataAnnotations.Schema;

namespace Ogma.Infrastructure.Persistence.SharedKernel.ValueObjectRecords;

[ComplexType]
public record BankAccountRecord(
    string Bank,
    string Iban,
    string Currency,
    string? Bic = null
);
