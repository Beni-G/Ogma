using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;
using Ogma.Infrastructure.Persistence.Orders.Contexts;
using Ogma.Infrastructure.Persistence.Orders.Extensions;

namespace Ogma.Infrastructure.Persistence.Orders.Repositories;

public class OrderTypeRepository : IOrderTypeRepository
{
    private readonly OrdersDbContext _ordersDbContext;
    private readonly IMapper _mapper;
    public OrderTypeRepository(OrdersDbContext ordersDbContext, IMapper mapper)
    {
        _ordersDbContext = ordersDbContext;
        _mapper = mapper;
    }
    public async Task<OrderType?> GetByIdAsync(long id)
    {
        var orderType = await _ordersDbContext.OrderTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(ot => ot.Id == id);
        return orderType?.ToDomain();
    }

    public async Task<OrderType> AddAsync(OrderType orderType)
    {
        var model = orderType.ToModel();
        await _ordersDbContext.OrderTypes.AddAsync(model);
        await _ordersDbContext.SaveChangesAsync();
        return model.ToDomain();
    }

    public async Task<bool> UpdateAsync(OrderType orderType)
    {
        var model = orderType.ToModel();
        _ordersDbContext.OrderTypes.Attach(model);
        _ordersDbContext.Entry(model).State = EntityState.Modified;

        var affected = await _ordersDbContext.SaveChangesAsync();
        return affected > 0;
    }

    public async Task DeleteAsync(OrderType orderType)
    {
        var model = orderType.ToModel();
        _ordersDbContext.OrderTypes.Attach(model);
        _ordersDbContext.OrderTypes.Remove(model);
        await _ordersDbContext.SaveChangesAsync();
    }
}
