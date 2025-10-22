using MediatR;

namespace Ogma.Application.Catalog.Commands;
public record DeleteItemCommand(long Id) : IRequest;
