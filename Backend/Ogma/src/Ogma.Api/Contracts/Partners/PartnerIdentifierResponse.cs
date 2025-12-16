using Ogma.Api.Contracts.SharedKernel;

namespace Ogma.Api.Contracts.Partners;

public record PartnerIdentifierResponse(
    string Type,
    string Value,
    PeriodResponse? ValidityPeriod,
    bool IsPrimary);

