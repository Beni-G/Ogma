using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Infrastructure.Persistence.Catalog.Contexts;
using Ogma.Infrastructure.Persistence.Catalog.Extensions;

namespace Ogma.Infrastructure.Persistence.Catalog.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly CatalogDbContext _catalogDbContext;

    public CategoryRepository(CatalogDbContext catalogDbContext) => _catalogDbContext = catalogDbContext;

    public async Task<Category?> GetByIdAsync(long id)
    {
        return await _catalogDbContext.Categories
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => c.ToDomain())
            .FirstOrDefaultAsync();
    }

    public async Task<Category> AddAsync(Category category)
    {
        string? path = null;
        if (category.ParentCategoryId.HasValue)
        {
            var parentCategory = await GetByIdAsync(category.ParentCategoryId.Value);
            if (parentCategory != null)
            {
                path = string.IsNullOrEmpty(parentCategory.Path)
                    ? parentCategory.Id.ToString()
                    : $"{parentCategory.Path}/{parentCategory.Id}";
            }
        }
        category.UpdatePath(path);
        var model = category.ToModel();
        await _catalogDbContext.Categories.AddAsync(model);
        await _catalogDbContext.SaveChangesAsync();
        return model.ToDomain();
    }

    public async Task<bool> UpdateAsync(Category category)
    {
        var model = await _catalogDbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == category.Id);

        if (model is null)
        {
            throw new InvalidOperationException($"Category with ID {category.Id} was not found.");
        }

        // Check if parent changed
        bool parentChanged = model.ParentCategoryId != category.ParentCategoryId;

        // Update basic fields
        model.Name = category.Name;
        model.ParentCategoryId = category.ParentCategoryId;

        if (parentChanged)
        {
            // Recalculate new path
            string? newParentPath = null;

            if (category.ParentCategoryId.HasValue)
            {
                var parent = await _catalogDbContext.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == category.ParentCategoryId.Value);

                if (parent is null)
                {
                    throw new InvalidOperationException($"Category with ID {category.ParentCategoryId.Value} was not found.");
                }

                newParentPath = string.IsNullOrEmpty(parent.Path)
                    ? parent.Id.ToString()
                    : $"{parent.Path}/{parent.Id}";
            }

            model.Path = newParentPath;

            // Load descendants
            var descendants = await GetDescendantsAsync(category.Id);

            // Build a lookup for current models
            var models = await _catalogDbContext.Categories
                .Where(c => descendants.Select(d => d.Id).Contains(c.Id))
                .ToListAsync();

            var modelLookup = models.ToDictionary(m => m.Id);

            // Recalculate paths for descendants
            foreach (var descendant in descendants)
            {
                var descendantModel = modelLookup[descendant.Id];

                var parentModel = modelLookup.GetValueOrDefault(descendantModel.ParentCategoryId ?? 0)
                    ?? (descendantModel.ParentCategoryId == category.Id ? model : null);

                if (parentModel is null)
                {
                    continue;
                }

                descendantModel.Path = string.IsNullOrEmpty(parentModel.Path)
                    ? parentModel.Id.ToString()
                    : $"{parentModel.Path}/{parentModel.Id}";
            }
        }

        var affected = await _catalogDbContext.SaveChangesAsync();
        return affected > 0;
    }

    public async Task DeleteAsync(Category category)
    {
        var model = category.ToModel();

        _catalogDbContext.Categories.Attach(model);
        _catalogDbContext.Categories.Remove(model);
        await _catalogDbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously retrieves all descendant categories of the specified root category.
    /// </summary>
    /// <param name="rootId">The identifier of the root category for which to retrieve all descendant categories.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of all descendant
    /// categories of the specified root category. The collection is empty if the root category has no descendants.</returns>
    private async Task<IEnumerable<Category>> GetDescendantsAsync(long rootId)
    {
        var result = new List<Category>();
        await CollectDescendantsAsync(rootId, result);
        return result;
    }

    /// <summary>
    /// Asynchronously retrieves all categories that are direct children of the specified parent category.
    /// </summary>
    /// <param name="ParentCategoryId">The unique identifier of the parent category whose child categories are to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of categories that are
    /// direct children of the specified parent category. The collection is empty if no child categories are found.</returns>
    private async Task<IEnumerable<Category>> GetChildrenAsync(long ParentCategoryId)
    {
        return await _catalogDbContext.Categories
            .AsNoTracking()
            .Where(c => c.ParentCategoryId == ParentCategoryId)
            .Select(c => c.ToDomain())
            .ToListAsync();
    }

    /// <summary>
    /// Collects all descendant categories recursively.
    /// </summary>
    /// <param name="parentCategoryId"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    private async Task CollectDescendantsAsync(long parentCategoryId, List<Category> result)
    {
        var children = await GetChildrenAsync(parentCategoryId);
        foreach (var child in children)
        {
            result.Add(child);
            await CollectDescendantsAsync(child.Id, result);
        }
    }
}
