using MediatR;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Extensions;
using Ogma.Domain.Partners.Repositories;

namespace Ogma.Application.Partners.Queries;

public class GetAllPartnersHandler : IRequestHandler<GetAllPartnersQuery, List<PartnerDto>>
{
    private readonly IPartnerRepository _partnerRepository;
    public GetAllPartnersHandler(IPartnerRepository partnerRepository) => _partnerRepository = partnerRepository;
    public async Task<List<PartnerDto>> Handle(GetAllPartnersQuery request, CancellationToken cancellationToken)
    {
        var partners = await _partnerRepository.GetAllAsync();
        return partners.Select(p => p.ToDto()).ToList();
    }
}
