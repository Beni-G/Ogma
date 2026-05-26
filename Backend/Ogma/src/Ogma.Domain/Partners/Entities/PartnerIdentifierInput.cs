using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Partners.Entities;

public record PartnerIdentifierInput(
    long Id,
    string Type,
    string Value,
    Period? ValidityPeriod,
    bool IsPrimary
);