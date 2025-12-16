using MediatR;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Extensions;
using Ogma.Domain.Partners.Repositories;

namespace Ogma.Application.Partners.Queries;

public class GetPartnerRoleTypeByIdHandler : IRequestHandler<GetPartnerRoleTypeByIdQuery, PartnerRoleTypeDto>
{
    private readonly IPartnerRoleTypeRepository _partnerRoleTypeRepository;

    public GetPartnerRoleTypeByIdHandler(IPartnerRoleTypeRepository partnerRoleTypeRepository)
    {
        _partnerRoleTypeRepository = partnerRoleTypeRepository;
    }
    public async Task<PartnerRoleTypeDto> Handle(GetPartnerRoleTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var partnerRoleType = await _partnerRoleTypeRepository.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException($"PartnerRoleType with Id {request.Id} not found.");
        return partnerRoleType.ToDto();
    }
}
