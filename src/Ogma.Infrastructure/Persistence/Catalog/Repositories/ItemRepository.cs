using Microsoft.EntityFrameworkCore;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Infrastructure.Persistence.Catalog.Contexts;
using Ogma.Infrastructure.Persistence.Catalog.Extensions;
using Ogma.Domain.Catalog.Entities;
using System.Linq.Expressions;

namespace Ogma.Infrastructure.Persistence.Catalog.Repositories;
public class ItemRepository : IItemRepository
{
    private readonly CatalogDbContext _catalogDbContext;

    public ItemRepository(CatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }
    public async Task<Item?> GetByIdAsync(long id)
    {
        return await _catalogDbContext.Items
            .AsNoTracking()
            .Where(i => i.Id == id)
            .Select(i => i.ToDomain())
            .FirstOrDefaultAsync(); 
    }

    public async Task<IEnumerable<Item>> GetAllAsync()
    {
        return await _catalogDbContext.Items
            .AsNoTracking()
            .Select(i => i.ToDomain())
            .ToListAsync();
    }

    public async Task<IEnumerable<Item>> GetAllAsync(Expression<Func<Item, bool>> predicate)
    {
        return await _catalogDbContext.Items
            .AsNoTracking()
            .Select(i => i.ToDomain())
            .Where(predicate)
            .ToListAsync();
    }

    public async Task AddAsync(Item item)
    {
        await _catalogDbContext.Items.AddAsync(item.ToModel());
        await _catalogDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Item item)
    {
        var model = item.ToModel(); 

        _catalogDbContext.Items.Attach(model); 
        _catalogDbContext.Entry(model).State = EntityState.Modified;

        await _catalogDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Item item)
    {
        var model = item.ToModel();

        _catalogDbContext.Items.Attach(model);
        _catalogDbContext.Entry(model).State = EntityState.Modified;

        await _catalogDbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _catalogDbContext.Items
            .AsNoTracking()
            .AnyAsync(i => i.Code == code);
    }
}
