using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Domain.Catalog.Services;

namespace Ogma.Application.Catalog.Commands;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryWithDescendantsDto>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICategoryDomainService _categoryDomainService;

    public UpdateCategoryHandler(ICategoryRepository categoryRepository, ICategoryDomainService categoryDomainService)
    {
        _categoryRepository = categoryRepository;
        _categoryDomainService = categoryDomainService;
    }

    public async Task<CategoryWithDescendantsDto> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.Id);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {command.Id} was not found.");
        }

        Category? parent = null;
        if (command.ParentCategoryId.HasValue)
        {
            parent = await _categoryRepository.GetByIdAsync(command.ParentCategoryId.Value);
            if (parent == null)
            {
                throw new KeyNotFoundException($"Parent category with ID {command.ParentCategoryId.Value} was not found.");
            }
        }

        category.Update(command.Name, command.ParentCategoryId);

        // Compute path using domain service
        var path = _categoryDomainService.ComputePath(category, parent);
        category.UpdatePath(path);

        var updated = await _categoryRepository.UpdateAsync(category);
        if (!updated)
        {
            throw new InvalidOperationException($"Update failed for category with ID {category.Id}.");
        }

        return category.ToDtoWithDescendants();
    }
}
