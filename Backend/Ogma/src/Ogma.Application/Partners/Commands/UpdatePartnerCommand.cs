using MediatR;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Application.Partners.Commands;

public record UpdatePartnerCommand(
    long Id,
    PersonNameDto? IndividualName,
    string? CompanyName,
    bool IsNaturalPerson,
    bool IsActive,
    string? DisplayName,
    AddressDto? HQAddress,
    IReadOnlyList<PartnerIdentifierDto> Identifiers,
    IReadOnlyList<long> RoleIds,
    IReadOnlyList<PartnerBankAccountDto> BankAccounts,
    IReadOnlyList<PartnerContactDto> Contacts) : IRequest<PartnerDto>;

