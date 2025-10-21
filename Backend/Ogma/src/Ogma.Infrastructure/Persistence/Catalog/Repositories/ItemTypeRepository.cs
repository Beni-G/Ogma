using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
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
    private readonly IMapper _mapper;

    public ItemTypeRepository(CatalogDbContext catalogDbContext, IMapper mapper)
    {
        _catalogDbContext = catalogDbContext;
        _mapper = mapper;
    }
    public async Task<ItemType?> GetByIdAsync(long id)
    {
        var entity = await _catalogDbContext.ItemTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(it => it.Id == id);
        return entity?.ToDomain();
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
        var modelPredicate = _mapper.MapExpression<Expression<Func<Models.ItemType, bool>>>(predicate);

        return await _catalogDbContext.ItemTypes
           .AsNoTracking()
           .Where(modelPredicate)
           .Select(it => it.ToDomain())
           .ToListAsync();
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
