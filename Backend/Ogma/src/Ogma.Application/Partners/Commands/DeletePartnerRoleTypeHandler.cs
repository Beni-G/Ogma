using MediatR;
using Ogma.Domain.Partners.Repositories;

namespace Ogma.Application.Partners.Commands;

public class DeletePartnerRoleTypeHandler : IRequestHandler<DeletePartnerRoleTypeCommand>
{
    private readonly IPartnerRoleTypeRepository _partnerRoleTypeRepository;

    public DeletePartnerRoleTypeHandler(IPartnerRoleTypeRepository partnerRoleTypeRepository) => _partnerRoleTypeRepository = partnerRoleTypeRepository;
    public async Task Handle(DeletePartnerRoleTypeCommand command, CancellationToken cancellationToken)
    {
        var existingPartnerRoleType = await _partnerRoleTypeRepository.GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"PartnerRoleType with Id {command.Id} not found.");

        await _partnerRoleTypeRepository.DeleteAsync(existingPartnerRoleType);
    }
}
