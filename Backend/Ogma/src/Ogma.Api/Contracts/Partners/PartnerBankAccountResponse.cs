using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Partners;

public record PartnerBankAccountResponse(BankAccountResponse BankAccount, bool IsDefault);
