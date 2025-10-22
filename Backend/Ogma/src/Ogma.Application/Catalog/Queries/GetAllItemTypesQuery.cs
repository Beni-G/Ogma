using MediatR;
using Ogma.Application.Catalog.Dtos;

namespace Ogma.Application.Catalog.Queries;
public record GetAllItemTypesQuery : IRequest<List<ItemTypeDto>>;
