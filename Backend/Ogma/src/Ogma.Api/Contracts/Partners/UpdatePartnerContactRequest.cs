using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Partners;

public record UpdatePartnerContactRequest(
    long Id,
    PersonNameRequest Name,
    string? Email,
    string? Phone,
    string? Mobile,
    string? Title,
    string? JobTitle,
    bool IsPrimary
);
