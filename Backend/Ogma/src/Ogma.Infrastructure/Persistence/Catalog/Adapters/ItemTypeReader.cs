using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Ports;
using Ogma.Infrastructure.Persistence.Catalog.Contexts;
using Ogma.Infrastructure.Persistence.Catalog.Extensions;
using System.Linq.Expressions;

namespace Ogma.Infrastructure.Persistence.Catalog.Adapters;

public class ItemTypeReader : IItemTypeReader
{
    private readonly CatalogDbContext _catalogDbContext;
    private readonly IMapper _mapper;

    public ItemTypeReader(CatalogDbContext catalogDbContext, IMapper mapper)
    {
        _catalogDbContext = catalogDbContext;
        _mapper = mapper;
    }

    public async Task<ItemTypeDto?> GetByIdAsync(long id)
    {
        var entity = await _catalogDbContext.ItemTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(it => it.Id == id);
        return entity?.ToDto();
    }

    public async Task<IDictionary<long, ItemTypeDto>> GetByIdsAsync(IEnumerable<long> ids)
    {
        return await _catalogDbContext.ItemTypes
            .AsNoTracking()
            .Where(it => ids.Contains(it.Id))
            .Select(it => it.ToDto())
            .ToDictionaryAsync(it => it.Id);
    }

    public async Task<IEnumerable<ItemTypeDto>> GetAllAsync()
    {
        return await _catalogDbContext.ItemTypes
           .AsNoTracking()
           .Select(it => it.ToDto())
           .ToListAsync();
    }

    public async Task<IEnumerable<ItemTypeDto>> GetAllAsync(Expression<Func<ItemTypeDto, bool>> predicate)
    {
        var modelPredicate = _mapper.MapExpression<Expression<Func<Models.ItemType, bool>>>(predicate);

        return await _catalogDbContext.ItemTypes
           .AsNoTracking()
           .Where(modelPredicate)
           .Select(it => it.ToDto())
           .ToListAsync();
    }
}
