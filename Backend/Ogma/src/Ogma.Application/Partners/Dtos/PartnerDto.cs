using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Application.Partners.Dtos;

public record PartnerDto(
    long Id,
    PersonNameDto? IndividualName,
    string? CompanyName,
    bool IsNaturalPerson,
    bool IsActive,
    string? DisplayName,
    AddressDto? HQAddress,
    IReadOnlyList<PartnerRoleTypeDto> Roles,
    IReadOnlyList<PartnerIdentifierDto> Identifiers,
    IReadOnlyList<PartnerBankAccountDto> BankAccounts,
    IReadOnlyList<PartnerContactDto> Contacts
);
