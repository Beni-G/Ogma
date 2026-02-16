using MediatR;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Ports;

namespace Ogma.Application.Partners.Queries;

public class GetPartnerByIdHandler : IRequestHandler<GetPartnerByIdQuery, PartnerDto>
{
    private readonly IPartnerReader _partnerReader;
    public GetPartnerByIdHandler(IPartnerReader partnerReader) => _partnerReader = partnerReader;

    public async Task<PartnerDto> Handle(GetPartnerByIdQuery query, CancellationToken cancellationToken) =>
        await _partnerReader.GetByIdAsync(query.Id) ?? throw new KeyNotFoundException($"Partner with ID {query.Id} was not found.");
}
