using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Partners;

public record UpdatePartnerBankAccountRequest(long Id, BankAccountRequest BankAccount, bool IsDefault);
