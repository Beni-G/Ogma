using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Partners;

public record PartnerBankAccountRequest(BankAccountRequest BankAccount, bool IsDefault);
