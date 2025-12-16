using MediatR;
using Ogma.Application.Partners.Dtos;

namespace Ogma.Application.Partners.Queries;

public record GetPartnerRoleTypeByIdQuery(long Id) : IRequest<PartnerRoleTypeDto>;
