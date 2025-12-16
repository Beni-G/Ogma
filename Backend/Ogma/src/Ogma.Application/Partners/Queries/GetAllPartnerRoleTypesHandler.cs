using MediatR;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Extensions;
using Ogma.Domain.Partners.Repositories;

namespace Ogma.Application.Partners.Queries;

public class GetAllPartnerRoleTypesHandler : IRequestHandler<GetAllPartnerRoleTypesQuery, List<PartnerRoleTypeDto>>
{
    private readonly IPartnerRoleTypeRepository _partnerRoleTypeRepository;

    public GetAllPartnerRoleTypesHandler(IPartnerRoleTypeRepository partnerRoleTypeRepository)
    {
        _partnerRoleTypeRepository = partnerRoleTypeRepository;
    }
    public async Task<List<PartnerRoleTypeDto>> Handle(GetAllPartnerRoleTypesQuery query, CancellationToken cancellationToken)
    {
        var partnerRoleTypes = await _partnerRoleTypeRepository.GetAllAsync();
        return partnerRoleTypes.Select(prt => prt.ToDto()).ToList();
    }
}
