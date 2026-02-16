using MediatR;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Application.Partners.Commands;

public record CreatePartnerCommand(
    PersonNameDto? IndividualName,
    string? CompanyName,
    bool IsNaturalPerson,
    AddressDto? HQAddress,
    CreatePartnerIdentifierDto Identifier,
    long RoleId) : IRequest<PartnerDto>;

