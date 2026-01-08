using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Infrastructure.Persistence.Catalog.Contexts;
using Ogma.Infrastructure.Persistence.Catalog.Extensions;
using System.Linq.Expressions;

namespace Ogma.Infrastructure.Persistence.Catalog.Repositories;
public class ItemRepository : IItemRepository
{
    private readonly CatalogDbContext _catalogDbContext;
    private readonly IMapper _mapper;

    public ItemRepository(CatalogDbContext catalogDbContext, IMapper mapper)
    {
        _catalogDbContext = catalogDbContext;
        _mapper = mapper;
    }
    public async Task<Item?> GetByIdAsync(long id)
    {
        return await _catalogDbContext.Items
            .Include(i => i.Category)
            .Include(i => i.ItemType)
            .AsNoTracking()
            .Where(i => i.Id == id)
            .Select(i => i.ToDomain())
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Item>> GetAllAsync()
    {
        return await _catalogDbContext.Items
            .Include(i => i.Category)
            .Include(i => i.ItemType)
            .AsNoTracking()
            .Select(i => i.ToDomain())
            .ToListAsync();
    }

    public async Task<IEnumerable<Item>> GetAllAsync(Expression<Func<Item, bool>> predicate)
    {
        var modelPredicate = _mapper.MapExpression<Expression<Func<Models.Item, bool>>>(predicate);

        return await _catalogDbContext.Items
            .Include(i => i.Category)
            .Include(i => i.ItemType)
            .AsNoTracking()
            .Where(modelPredicate)
            .Select(i => i.ToDomain())
            .ToListAsync();
    }

    public async Task<Item> AddAsync(Item item)
    {
        var model = item.ToModel();
        await _catalogDbContext.Items.AddAsync(model);
        await _catalogDbContext.SaveChangesAsync();

        return model.ToDomain(); ;
    }

    public async Task<bool> UpdateAsync(Item item)
    {
        var model = item.ToModel();

        _catalogDbContext.Items.Attach(model);
        _catalogDbContext.Entry(model).State = EntityState.Modified;

        var affected = await _catalogDbContext.SaveChangesAsync();
        return affected > 0;
    }

    public async Task DeleteAsync(Item item)
    {
        var model = item.ToModel();

        _catalogDbContext.Items.Attach(model);
        _catalogDbContext.Items.Remove(model);

        await _catalogDbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _catalogDbContext.Items
            .AsNoTracking()
            .AnyAsync(i => i.Code == code);
    }
}
