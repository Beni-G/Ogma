using MediatR;
using Ogma.Application.Catalog.Dtos;

namespace Ogma.Application.Catalog.Commands;
public record UpdateCategoryCommand(long Id, string Name, long? ParentCategoryId) : IRequest<CategoryDto>;
