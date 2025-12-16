using MediatR;
using Ogma.Application.Partners.Dtos;

namespace Ogma.Application.Partners.Queries;

public record GetAllPartnerRoleTypesQuery : IRequest<List<PartnerRoleTypeDto>>;
