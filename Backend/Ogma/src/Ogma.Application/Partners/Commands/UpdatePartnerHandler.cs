using MediatR;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Extensions;
using Ogma.Application.SharedKernel.Extensions;
using Ogma.Domain.Partners.Repositories;

namespace Ogma.Application.Partners.Commands;

public class UpdatePartnerHandler : IRequestHandler<UpdatePartnerCommand, PartnerDto>
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly IPartnerRoleTypeRepository _partnerRoleTypeRepository;

    public UpdatePartnerHandler(IPartnerRepository partnerRepository, IPartnerRoleTypeRepository partnerRoleTypeRepository)
    {
        _partnerRepository = partnerRepository;
        _partnerRoleTypeRepository = partnerRoleTypeRepository;
    }
    public async Task<PartnerDto> Handle(UpdatePartnerCommand command, CancellationToken cancellationToken)
    {
        var existingPartner = await _partnerRepository.GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"Partner with ID {command.Id} was not found.");

        var roles = await _partnerRoleTypeRepository.GetAllAsync(prt => command.RoleIds.Contains(prt.Id));

        if (!roles.Any())
        {
            throw new InvalidOperationException("At least one valid partner role type must be provided.");
        }

        existingPartner.Update(
            command.IndividualName!.ToDomain(),
            command.CompanyName,
            command.IsNaturalPerson,
            command.IsActive,
            command.DisplayName,
            command.HQAddress!.ToDomain(),
            command.Identifiers.Select(id => id.ToDomain()).ToList(),
            roles.ToList(),
            command.BankAccounts.Select(ba => ba.ToDomain()).ToList(),
            command.Contacts.Select(c => c.ToDomain()).ToList());

        var result = await _partnerRepository.UpdateAsync(existingPartner);

        if (!result)
        {
            throw new InvalidOperationException($"Update failed for Partner with ID {command.Id}.");
        }

        return existingPartner.ToDto();
    }
}
