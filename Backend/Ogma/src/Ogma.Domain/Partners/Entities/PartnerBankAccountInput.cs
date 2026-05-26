using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Partners.Entities;

public record PartnerBankAccountInput(long Id, BankAccount BankAccount, bool IsDefault);