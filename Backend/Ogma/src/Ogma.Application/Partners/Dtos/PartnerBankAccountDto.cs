using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Application.Partners.Dtos;

public record PartnerBankAccountDto(long Id, BankAccountDto BankAccount, bool IsDefault);
