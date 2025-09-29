using Microsoft.EntityFrameworkCore;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Infrastructure.Persistence.Catalog.Contexts;
using Ogma.Infrastructure.Persistence.Catalog.Extensions;
using System.Linq.Expressions;

namespace Ogma.Infrastructure.Persistence.Catalog.Repositories;
public class CategoryRepository : ICategoryRepository
{
    private readonly CatalogDbContext _catalogDbContext;

    public CategoryRepository(CatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public async Task<Category?> GetByIdAsync(long id)
    {
        return await _catalogDbContext.Categories
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => c.ToDomain())
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _catalogDbContext.Categories
            .AsNoTracking()
            .Select(c => c.ToDomain())
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>> predicate)
    {
        return await _catalogDbContext.Categories
            .AsNoTracking()
            .Select(c => c.ToDomain())
            .Where(predicate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetChildrenAsync(long ParentCategoryId)
    {
        return await _catalogDbContext.Categories
            .AsNoTracking()
            .Where(c => c.ParentCategoryId == ParentCategoryId)
            .Select(c => c.ToDomain())
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetDescendantsAsync(long rootId)
    {
        var result = new List<Category>();
        await CollectDescendantsAsync(rootId, result);
        return result;
    }

    public async Task<IEnumerable<Category>> GetAncestorsAsync(long leafId)
    {
        var categoryModel = await _catalogDbContext.Categories
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == leafId);

        if (categoryModel is null)
        {
            throw new InvalidOperationException($"Category with ID {categoryModel.Id} was not found.");
        }

        var ids = categoryModel.Path?.Split('/')
            .Select(long.Parse)
            .ToList() ?? new();

        if (!ids.Any())
        {
            return [];
        }

        var models = await _catalogDbContext.Categories
            .Where(c => ids.Contains(c.Id))
            .ToListAsync();

        var ordered = models
            .OrderBy(m => ids.IndexOf(m.Id))
            .Select(m => m.ToDomain())
            .ToList();

        return ordered;
    }

    public async Task AddAsync(Category category)
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
        await _catalogDbContext.Categories.AddAsync(category.ToModel());
        await _catalogDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
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

        await _catalogDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category category)
    {
        var model = category.ToModel();

        _catalogDbContext.Categories.Attach(model);
        _catalogDbContext.Categories.Remove(model);
        await _catalogDbContext.SaveChangesAsync();
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
