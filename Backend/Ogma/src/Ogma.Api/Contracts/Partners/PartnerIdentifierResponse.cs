using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Partners;

public record PartnerIdentifierResponse(
    long Id,
    string Type,
    string Value,
    PeriodResponse? ValidityPeriod,
    bool IsPrimary
);

