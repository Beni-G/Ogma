using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Ports;
using Ogma.Infrastructure.Persistence.Catalog.Contexts;
using Ogma.Infrastructure.Persistence.Catalog.Extensions;
using System.Linq.Expressions;

namespace Ogma.Infrastructure.Persistence.Catalog.Adapters;

public class ItemReader : IItemReader
{
    private readonly CatalogDbContext _catalogDbContext;
    private readonly IMapper _mapper;

    public ItemReader(CatalogDbContext catalogDbContext, IMapper mapper)
    {
        _catalogDbContext = catalogDbContext;
        _mapper = mapper;
    }

    public async Task<ItemDto?> GetByIdAsync(long id)
    {
        return (await _catalogDbContext.Items
            .Include(i => i.Category)
            .Include(i => i.ItemType)
            .AsNoTracking()
            .Where(i => i.Id == id)
            .FirstOrDefaultAsync())?.ToDto();
    }

    public async Task<IDictionary<long, ItemDto>> GetByIdsAsync(IEnumerable<long> ids)
    {
        return (await _catalogDbContext.Items
            .Include(i => i.Category)
            .Include(i => i.ItemType)
            .AsNoTracking()
            .Where(i => ids.Contains(i.Id))
            .ToListAsync())
            .ToDictionary(i => i.Id, i => i.ToDto());
    }

    public async Task<IEnumerable<ItemDto>> GetAllAsync()
    {
        return (await _catalogDbContext.Items
            .Include(i => i.Category)
            .Include(i => i.ItemType)
            .AsNoTracking()
            .ToListAsync())
            .Select(i => i.ToDto());
    }

    public async Task<IEnumerable<ItemDto>> GetAllAsync(Expression<Func<ItemDto, bool>> predicate)
    {
        var modelPredicate = _mapper.MapExpression<Expression<Func<Models.Item, bool>>>(predicate);

        return (await _catalogDbContext.Items
            .Include(i => i.Category)
            .Include(i => i.ItemType)
            .AsNoTracking()
            .Where(modelPredicate)
            .ToListAsync())
            .Select(i => i.ToDto());
    }
}
