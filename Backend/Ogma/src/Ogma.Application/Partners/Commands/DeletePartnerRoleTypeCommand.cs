using MediatR;

namespace Ogma.Application.Partners.Commands;

public record DeletePartnerRoleTypeCommand(long Id) : IRequest;

