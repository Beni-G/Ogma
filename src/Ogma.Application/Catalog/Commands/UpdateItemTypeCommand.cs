using MediatR;
using Ogma.Application.Catalog.Dtos;

namespace Ogma.Application.Catalog.Commands;
public sealed record UpdateItemTypeCommand(long Id, string Name, string Description) : IRequest<ItemTypeDto>;
