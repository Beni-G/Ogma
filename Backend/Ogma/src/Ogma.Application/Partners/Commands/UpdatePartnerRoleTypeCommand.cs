using MediatR;
using Ogma.Application.Partners.Dtos;

namespace Ogma.Application.Partners.Commands;

public record UpdatePartnerRoleTypeCommand(long Id, string Code, string Name, string? Color) : IRequest<PartnerRoleTypeDto>;
