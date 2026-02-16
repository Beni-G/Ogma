using Microsoft.EntityFrameworkCore;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;
using Ogma.Infrastructure.Persistence.Catalog.Contexts;

namespace Ogma.Infrastructure.Persistence.Orders.Adapters;

public class CatalogItemReader : ICatalogItemReader
{
    private readonly CatalogDbContext _catalogDbContext;
    public CatalogItemReader(CatalogDbContext catalogDbContext) => _catalogDbContext = catalogDbContext;

    public async Task<OrderItemDto?> GetByIdAsync(long id)
    {
        var item = await _catalogDbContext.Items
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);

        var orderItem = item != null ? new OrderItemDto(item.Id, item.Name, item.Code) : null;

        return orderItem;
    }

    public async Task<IDictionary<long, OrderItemDto>> GetByIdsAsync(IEnumerable<long> ids)
    {
        return await _catalogDbContext.Items
            .AsNoTracking()
            .Where(i => ids.Contains(i.Id))
            .Select(i => new OrderItemDto(i.Id, i.Name, i.Code))
            .ToDictionaryAsync(i => i.ItemId);
    }

}
