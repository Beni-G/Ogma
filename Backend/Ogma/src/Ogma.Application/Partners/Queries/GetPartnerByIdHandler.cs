using MediatR;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Extensions;
using Ogma.Domain.Partners.Repositories;

namespace Ogma.Application.Partners.Queries;

public class GetPartnerByIdHandler : IRequestHandler<GetPartnerByIdQuery, PartnerDto>
{
    private readonly IPartnerRepository _partnerRepository;
    public GetPartnerByIdHandler(IPartnerRepository partnerRepository) => _partnerRepository = partnerRepository;

    public async Task<PartnerDto> Handle(GetPartnerByIdQuery query, CancellationToken cancellationToken)
    {
        var partner = await _partnerRepository.GetByIdAsync(query.Id);
        if (partner == null)
        {
            throw new KeyNotFoundException($"Partner with ID {query.Id} was not found.");
        }
        return partner.ToDto();
    }
}
