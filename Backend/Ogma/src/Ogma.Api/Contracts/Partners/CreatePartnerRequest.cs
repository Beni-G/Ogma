using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Partners;

public record CreatePartnerRequest(
    PersonNameRequest? IndividualName,
    string? CompanyName,
    bool IsNaturalPerson,
    AddressRequest HQAddress,
    PartnerIdentifierRequest Identifier,
    long RoleId);