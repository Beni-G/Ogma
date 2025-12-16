using MediatR;
using Ogma.Application.Partners.Dtos;

namespace Ogma.Application.Partners.Queries;

public record GetPartnerByIdQuery(long Id) : IRequest<PartnerDto>;
