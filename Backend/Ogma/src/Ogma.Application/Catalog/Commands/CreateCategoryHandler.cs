using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Extensions;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Domain.Catalog.Services;

namespace Ogma.Application.Catalog.Commands;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryWithDescendantsDto>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICategoryDomainService _categoryDomainService;

    public CreateCategoryHandler(ICategoryRepository categoryRepository, ICategoryDomainService categoryDomainService)
    {
        _categoryRepository = categoryRepository;
        _categoryDomainService = categoryDomainService;
    }

    public async Task<CategoryWithDescendantsDto> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        Category? parent = null;

        if (command.ParentCategoryId.HasValue)
        {
            parent = await _categoryRepository.GetByIdAsync(command.ParentCategoryId.Value);
            if (parent == null)
            {
                throw new KeyNotFoundException($"Parent category with ID {command.ParentCategoryId.Value} not found.");
            }
        }

        var newCategory = Category.Create(command.Name, command.ParentCategoryId);

        var path = _categoryDomainService.ComputePath(newCategory, parent);
        newCategory.UpdatePath(path);

        var created = await _categoryRepository.AddAsync(newCategory);
        return created.ToDtoWithDescendants();
    }
}
