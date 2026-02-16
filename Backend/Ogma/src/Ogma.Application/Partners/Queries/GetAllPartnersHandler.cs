using MediatR;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Ports;

namespace Ogma.Application.Partners.Queries;

public class GetAllPartnersHandler : IRequestHandler<GetAllPartnersQuery, List<PartnerDto>>
{
    private readonly IPartnerReader _partnerReader;
    public GetAllPartnersHandler(IPartnerReader partnerReader) => _partnerReader = partnerReader;
    public async Task<List<PartnerDto>> Handle(GetAllPartnersQuery request, CancellationToken cancellationToken) =>
        (await _partnerReader.GetAllAsync()).ToList();
}
