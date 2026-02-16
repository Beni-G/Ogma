using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Partners;

public record PartnerContactResponse(
    long Id,
    PersonNameResponse Name,
    string? Email,
    string? Phone,
    string? Mobile,
    string? Title,
    string? JobTitle,
    bool IsPrimary);
