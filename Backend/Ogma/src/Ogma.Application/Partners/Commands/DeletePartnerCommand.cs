using MediatR;

namespace Ogma.Application.Partners.Commands;

public record DeletePartnerCommand(long Id) : IRequest;
