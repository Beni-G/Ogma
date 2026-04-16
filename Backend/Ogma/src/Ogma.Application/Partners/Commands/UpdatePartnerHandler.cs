using MediatR;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Extensions;
using Ogma.Application.Partners.Ports;
using Ogma.Application.SharedKernel.Extensions;
using Ogma.Domain.Partners.Repositories;

namespace Ogma.Application.Partners.Commands;

public class UpdatePartnerHandler : IRequestHandler<UpdatePartnerCommand, PartnerDto>
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly IPartnerReader _partnerReader;
    private readonly IPartnerRoleTypeReader _partnerRoleTypeReader;

    public UpdatePartnerHandler(IPartnerRepository partnerRepository, IPartnerReader partnerReader, IPartnerRoleTypeReader partnerRoleTypeReader)
    {
        _partnerRepository = partnerRepository;
        _partnerReader = partnerReader;
        _partnerRoleTypeReader = partnerRoleTypeReader;
    }
    public async Task<PartnerDto> Handle(UpdatePartnerCommand command, CancellationToken cancellationToken)
    {
        var existingPartner = await _partnerRepository.GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"Partner with ID {command.Id} was not found.");

        var roles = await _partnerRoleTypeReader.GetAllAsync(prt => command.RoleIds.Contains(prt.Id));

        if (!roles.Any())
        {
            throw new InvalidOperationException("At least one valid partner role type must be provided.");
        }

        existingPartner.Update(
            command.IndividualName != null ? command.IndividualName.ToDomain() : null,
            command.CompanyName,
            command.IsNaturalPerson,
            command.IsActive,
            command.DisplayName,
            command.HQAddress!.ToDomain(),
            command.Identifiers.Select(id => id.ToDomain()).ToList(),
            roles.Select(r => r.Id).Distinct().ToList(),
            command.BankAccounts.Select(ba => ba.ToDomain()).ToList(),
            command.Contacts.Select(c => c.ToDomain()).ToList());

        var result = await _partnerRepository.UpdateAsync(existingPartner);

        if (!result)
        {
            throw new InvalidOperationException($"Update failed for Partner with ID {command.Id}.");
        }

        var updatedPartner = await _partnerReader.GetByIdAsync(command.Id)
            ?? throw new InvalidOperationException($"Failed to retrieve updated Partner with ID {command.Id}.");

        return updatedPartner;
    }
}
