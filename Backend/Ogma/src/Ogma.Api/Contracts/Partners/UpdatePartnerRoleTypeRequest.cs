namespace Ogma.Api.Contracts.Partners;

public record UpdatePartnerRoleTypeRequest(long Id, string Code, string Name, string? Color);
