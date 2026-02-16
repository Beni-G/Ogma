using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Partners;

public record PartnerResponse(
    long Id,
    PersonNameResponse? IndividualName,
    string? CompanyName,
    bool IsNaturalPerson,
    bool IsActive,
    string? DisplayName,
    AddressResponse? HQAddress,
    IReadOnlyList<PartnerRoleTypeResponse> Roles,
    IReadOnlyList<PartnerIdentifierResponse> Identifiers,
    IReadOnlyList<PartnerBankAccountResponse> BankAccounts,
    IReadOnlyList<PartnerContactResponse> Contacts
);
