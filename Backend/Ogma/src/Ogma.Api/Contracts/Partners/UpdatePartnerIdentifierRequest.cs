using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Partners;

public record UpdatePartnerIdentifierRequest(
    long Id,
    string Type,
    string Value,
    PeriodRequest? ValidityPeriod,
    bool IsPrimary
);
