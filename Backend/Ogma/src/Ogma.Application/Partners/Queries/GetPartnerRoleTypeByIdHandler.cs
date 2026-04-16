using MediatR;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Ports;

namespace Ogma.Application.Partners.Queries;

public class GetPartnerRoleTypeByIdHandler : IRequestHandler<GetPartnerRoleTypeByIdQuery, PartnerRoleTypeDto>
{
    private readonly IPartnerRoleTypeReader _partnerRoleTypeReader;

    public GetPartnerRoleTypeByIdHandler(IPartnerRoleTypeReader partnerRoleTypeReader)
    {
        _partnerRoleTypeReader = partnerRoleTypeReader;
    }
    public async Task<PartnerRoleTypeDto> Handle(GetPartnerRoleTypeByIdQuery request, CancellationToken cancellationToken) =>
        await _partnerRoleTypeReader.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException($"PartnerRoleType with Id {request.Id} not found.");

}
