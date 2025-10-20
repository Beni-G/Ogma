using MediatR;
using Ogma.Application.Catalog.Dtos;

namespace Ogma.Application.Catalog.Commands;
public record CreateItemTypeCommand(string Name, string Description) : IRequest<ItemTypeDto>;

