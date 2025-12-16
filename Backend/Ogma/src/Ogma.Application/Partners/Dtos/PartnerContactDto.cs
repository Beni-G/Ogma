using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Application.Partners.Dtos;

public record PartnerContactDto(
    PersonNameDto Name,
    string? Email,
    string? Phone,
    string? Mobile,
    string? Title,
    string? JobTitle,
    bool IsPrimary);
