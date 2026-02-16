using MediatR;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Ports;

namespace Ogma.Application.Partners.Queries;

public class GetAllPartnerRoleTypesHandler : IRequestHandler<GetAllPartnerRoleTypesQuery, List<PartnerRoleTypeDto>>
{
    private readonly IPartnerRoleTypeReader _partnerRoleTypeReader;

    public GetAllPartnerRoleTypesHandler(IPartnerRoleTypeReader partnerRoleTypeReader) => _partnerRoleTypeReader = partnerRoleTypeReader;

    public async Task<List<PartnerRoleTypeDto>> Handle(GetAllPartnerRoleTypesQuery query, CancellationToken cancellationToken) =>
        (await _partnerRoleTypeReader.GetAllAsync()).ToList();
}
