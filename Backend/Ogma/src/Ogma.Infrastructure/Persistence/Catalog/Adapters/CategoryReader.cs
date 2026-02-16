using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Ports;
using Ogma.Infrastructure.Persistence.Catalog.Contexts;
using Ogma.Infrastructure.Persistence.Catalog.Extensions;
using Ogma.Infrastructure.Persistence.Catalog.Models;
using System.Linq.Expressions;

namespace Ogma.Infrastructure.Persistence.Catalog.Adapters;

public class CategoryReader : ICategoryReader
{
    private readonly CatalogDbContext _catalogDbContext;
    private readonly IMapper _mapper;

    public CategoryReader(CatalogDbContext catalogDbContext, IMapper mapper)
    {
        _catalogDbContext = catalogDbContext;
        _mapper = mapper;
    }

    public async Task<CategoryDto?> GetByIdAsync(long id)
    {
        return (await _catalogDbContext.Categories
            .Include(c => c.SubCategories)
            .AsNoTracking()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync())?.ToDto();
    }

    public async Task<IDictionary<long, CategoryDto>> GetByIdsAsync(IEnumerable<long> ids)
    {
        var categories = await _catalogDbContext.Categories
            .Include(c => c.SubCategories)
            .AsNoTracking()
            .Where(c => ids.Contains(c.Id))
            .ToListAsync();

        return categories.Select(c => c.ToDto()).ToDictionary(c => c.Id);
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _catalogDbContext.Categories
            .Include(c => c.SubCategories)
            .AsNoTracking()
            .ToListAsync();

        return categories.Select(c => c.ToDto());
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(Expression<Func<CategoryDto, bool>> predicate)
    {
        var modelPredicate = _mapper.MapExpression<Expression<Func<Category, bool>>>(predicate);

        var categories = await _catalogDbContext.Categories
            .Include(c => c.SubCategories)
            .AsNoTracking()
            .Where(modelPredicate)
            .ToListAsync();

        return categories.Select(c => c.ToDto());
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesTreeAsync()
    {
        var roots = await _catalogDbContext.Categories
            .Include(c => c.SubCategories)
            .ThenInclude(sc => sc.SubCategories)
            .ThenInclude(ssc => ssc.SubCategories)
            .ThenInclude(sssc => sssc.SubCategories)
            .AsNoTracking()
            .Where(c => c.ParentCategoryId == null)
            .ToListAsync();

        return roots.Select(c => c.ToDtoRecursive());
    }

    public async Task<IEnumerable<CategoryDto>> GetChildrenAsync(long ParentCategoryId)
    {
        var categories = await _catalogDbContext.Categories
            .AsNoTracking()
            .Where(c => c.ParentCategoryId == ParentCategoryId)
            .ToListAsync();

        return categories.Select(c => c.ToDto());
    }

    public async Task<IEnumerable<CategoryDto>> GetDescendantsAsync(long rootId)
    {
        var result = new List<CategoryDto>();
        await CollectDescendantsAsync(rootId, result);
        return result;
    }

    public async Task<IEnumerable<CategoryDto>> GetAncestorsAsync(long leafId)
    {
        var categoryModel = await _catalogDbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == leafId) ?? throw new KeyNotFoundException($"Category with ID {leafId} was not found.");

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
            .Select(m => m.ToDto())
            .ToList();

        return ordered;
    }

    public async Task<IDictionary<long, IEnumerable<CategoryDto>>> GetAncestorsAsync(IEnumerable<long> leafIds)
    {
        var categories = await _catalogDbContext.Categories
            .AsNoTracking()
            .Where(c => leafIds.Contains(c.Id))
            .ToListAsync();
        var result = new Dictionary<long, IEnumerable<CategoryDto>>();
        foreach (var category in categories)
        {
            var ids = category.Path?.Split('/')
                .Select(long.Parse)
                .ToList() ?? new();
            if (!ids.Any())
            {
                result[category.Id] = [];
                continue;
            }
            var ancestorModels = await _catalogDbContext.Categories
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();
            var orderedAncestors = ancestorModels
                .OrderBy(m => ids.IndexOf(m.Id))
                .Select(m => m.ToDto())
                .ToList();
            result[category.Id] = orderedAncestors;
        }
        return result;
    }

    /// <summary>
    /// Collects all descendant categories recursively.
    /// </summary>
    /// <param name="parentCategoryId"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    private async Task CollectDescendantsAsync(long parentCategoryId, List<CategoryDto> result)
    {
        var children = await GetChildrenAsync(parentCategoryId);
        foreach (var child in children)
        {
            result.Add(child);
            await CollectDescendantsAsync(child.Id, result);
        }
    }
}
