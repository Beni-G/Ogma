using MediatR;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Extensions;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.Partners.Repositories;

namespace Ogma.Application.Partners.Commands;

public class CreatePartnerRoleTypeHandler : IRequestHandler<CreatePartnerRoleTypeCommand, PartnerRoleTypeDto>
{
    private readonly IPartnerRoleTypeRepository _partnerRoleTypeRepository;

    public CreatePartnerRoleTypeHandler(IPartnerRoleTypeRepository partnerRoleTypeRepository) => _partnerRoleTypeRepository = partnerRoleTypeRepository;

    public async Task<PartnerRoleTypeDto> Handle(CreatePartnerRoleTypeCommand request, CancellationToken cancellationToken)
    {
        var newPartnerRoleType = PartnerRoleType.Create(
            request.Code,
            request.Name,
            request.Color
        );
        var created = await _partnerRoleTypeRepository.AddAsync(newPartnerRoleType);
        return created.ToDto();
    }
}
