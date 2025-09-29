using Microsoft.EntityFrameworkCore;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Infrastructure.Persistence.Catalog.Contexts;
using Ogma.Infrastructure.Persistence.Catalog.Extensions;
using System.Linq.Expressions;

namespace Ogma.Infrastructure.Persistence.Catalog.Repositories;
public class ItemTypeRepository : IItemTypeRepository
{
    private readonly CatalogDbContext _catalogDbContext;

    public ItemTypeRepository(CatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }
    public async Task<ItemType?> GetByIdAsync(long id)
    {
        return await _catalogDbContext.ItemTypes
           .AsNoTracking()
           .Where(it => it.Id == id)
           .Select(it => it.ToDomain())
           .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ItemType>> GetAllAsync()
    {
        return await _catalogDbContext.ItemTypes
           .AsNoTracking()
           .Select(it => it.ToDomain())
           .ToListAsync();
    }

    public async Task<IEnumerable<ItemType>> GetAllAsync(Expression<Func<ItemType, bool>> predicate)
    {
        return await _catalogDbContext.ItemTypes
           .AsNoTracking()
           .Select(it => it.ToDomain())
           .Where(predicate)
           .ToListAsync();
    }

    public async Task AddAsync(ItemType itemType)
    {
        await _catalogDbContext.ItemTypes.AddAsync(itemType.ToModel());
        await _catalogDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(ItemType itemType)
    {
        _catalogDbContext.ItemTypes.Attach(itemType.ToModel());
        _catalogDbContext.Entry(itemType.ToModel()).State = EntityState.Modified;

        await _catalogDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(ItemType itemType)
    {
        var model = itemType.ToModel();

        _catalogDbContext.ItemTypes.Attach(model);
        _catalogDbContext.ItemTypes.Remove(model);

        await _catalogDbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _catalogDbContext.ItemTypes
            .AsNoTracking()
            .AnyAsync(it => it.Name == name);
    }
}
