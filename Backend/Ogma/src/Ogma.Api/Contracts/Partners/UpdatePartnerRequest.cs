using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Partners;

public record UpdatePartnerRequest(
    long Id,
    PersonNameRequest? IndividualName,
    string? CompanyName,
    bool IsNaturalPerson,
    bool IsActive,
    string? DisplayName,
    AddressRequest HQAddress,
    List<PartnerIdentifierRequest> Identifiers,
    List<long> RoleIds,
    List<PartnerBankAccountRequest> BankAccounts,
    List<PartnerContactRequest> Contacts);
