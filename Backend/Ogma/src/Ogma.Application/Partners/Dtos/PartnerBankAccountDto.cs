using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Application.Partners.Dtos;

public record PartnerBankAccountDto(BankAccountDto BankAccount, bool IsDefault);
