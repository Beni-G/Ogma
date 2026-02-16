using Microsoft.EntityFrameworkCore;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Infrastructure.Persistence.Catalog.Contexts;
using Ogma.Infrastructure.Persistence.Catalog.Extensions;

namespace Ogma.Infrastructure.Persistence.Catalog.Repositories;

public class ItemTypeRepository : IItemTypeRepository
{
    private readonly CatalogDbContext _catalogDbContext;

    public ItemTypeRepository(CatalogDbContext catalogDbContext) => _catalogDbContext = catalogDbContext;
    public async Task<ItemType?> GetByIdAsync(long id)
    {
        var entity = await _catalogDbContext.ItemTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(it => it.Id == id);
        return entity?.ToDomain();
    }

    public async Task<ItemType> AddAsync(ItemType itemType)
    {
        var model = itemType.ToModel();
        await _catalogDbContext.ItemTypes.AddAsync(model);
        await _catalogDbContext.SaveChangesAsync();
        return model.ToDomain();
    }

    public async Task<bool> UpdateAsync(ItemType itemType)
    {
        var model = itemType.ToModel();
        _catalogDbContext.ItemTypes.Attach(model);
        _catalogDbContext.Entry(model).State = EntityState.Modified;

        var affected = await _catalogDbContext.SaveChangesAsync();
        return affected > 0;
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
