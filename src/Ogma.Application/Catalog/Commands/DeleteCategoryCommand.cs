using MediatR;

namespace Ogma.Application.Catalog.Commands;
public record DeleteCategoryCommand(long Id) : IRequest;