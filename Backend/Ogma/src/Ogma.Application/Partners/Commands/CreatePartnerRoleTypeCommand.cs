using MediatR;
using Ogma.Application.Partners.Dtos;

namespace Ogma.Application.Partners.Commands;

public record CreatePartnerRoleTypeCommand(string Code, string Name, string? Color) : IRequest<PartnerRoleTypeDto>;
