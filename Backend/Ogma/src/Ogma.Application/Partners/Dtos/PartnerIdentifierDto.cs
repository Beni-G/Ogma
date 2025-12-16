using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Application.Partners.Dtos;

public record PartnerIdentifierDto(
    string Type,
    string Value,
    PeriodDto? ValidityPeriod,
    bool IsPrimary);
