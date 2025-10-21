using MediatR;

namespace Ogma.Application.Catalog.Commands;
public record DeleteItemTypeCommand(long Id) : IRequest;
