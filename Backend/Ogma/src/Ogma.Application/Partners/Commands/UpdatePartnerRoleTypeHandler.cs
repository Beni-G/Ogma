using MediatR;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Extensions;
using Ogma.Domain.Partners.Repositories;

namespace Ogma.Application.Partners.Commands;

public class UpdatePartnerRoleTypeHandler : IRequestHandler<UpdatePartnerRoleTypeCommand, PartnerRoleTypeDto>
{
    private readonly IPartnerRoleTypeRepository _partnerRoleTypeRepository;

    public UpdatePartnerRoleTypeHandler(IPartnerRoleTypeRepository partnerRoleTypeRepository) => _partnerRoleTypeRepository = partnerRoleTypeRepository;
    public async Task<PartnerRoleTypeDto> Handle(UpdatePartnerRoleTypeCommand command, CancellationToken cancellationToken)
    {
        var existingPartnerRoleType = await _partnerRoleTypeRepository.GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"PartnerRoleType with ID {command.Id} was not found.");

        existingPartnerRoleType.Update(command.Code, command.Name, command.Color);

        var result = await _partnerRoleTypeRepository.UpdateAsync(existingPartnerRoleType);
        if (!result)
        {
            throw new InvalidOperationException($"Update failed for PartnerRoleType with ID {command.Id}.");
        }

        return existingPartnerRoleType.ToDto();
    }
}
