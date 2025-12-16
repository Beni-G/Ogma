using MediatR;
using Ogma.Application.Partners.Dtos;

namespace Ogma.Application.Partners.Queries;

public record GetAllPartnersQuery : IRequest<List<PartnerDto>>;