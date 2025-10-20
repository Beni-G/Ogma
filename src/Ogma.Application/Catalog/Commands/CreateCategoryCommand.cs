using MediatR;
using Ogma.Application.Catalog.Dtos;

namespace Ogma.Application.Catalog.Commands;
public record CreateCategoryCommand(string Name, long? ParentCategoryId = null) : IRequest<CategoryWithDescendantsDto>;
