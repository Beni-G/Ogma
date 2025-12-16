using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Partners;

public record PartnerIdentifierRequest(
    string Type,
    string Value,
    PeriodRequest? ValidityPeriod,
    bool IsPrimary);
