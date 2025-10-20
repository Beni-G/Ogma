using MediatR;
using Ogma.Application.Catalog.Dtos;

namespace Ogma.Application.Catalog.Queries;
public record GetCategoryByIdQuery(long Id) : IRequest<CategoryWithDescendantsDto>;
