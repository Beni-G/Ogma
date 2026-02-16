using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Partners;

public record CreatePartnerIdentifierRequest(
    string Type,
    string Value,
    PeriodRequest? ValidityPeriod,
    bool IsPrimary
);
