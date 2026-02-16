using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Partners;

public record PartnerBankAccountResponse(long Id, BankAccountResponse BankAccount, bool IsDefault);
