using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;
using Ogma.Infrastructure.Persistence.Orders.Contexts;
using Ogma.Infrastructure.Persistence.Orders.Extensions;

namespace Ogma.Infrastructure.Persistence.Orders.Repositories;

public class OrderStatusRepository : IOrderStatusRepository
{
    private readonly OrdersDbContext _ordersDbContext;
    private readonly IMapper _mapper;

    public OrderStatusRepository(OrdersDbContext ordersDbContext, IMapper mapper)
    {
        _ordersDbContext = ordersDbContext;
        _mapper = mapper;
    }

    public async Task<OrderStatus?> GetByIdAsync(long id)
    {
        var orderStatus = await _ordersDbContext.OrderStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(os => os.Id == id);
        return orderStatus?.ToDomain();
    }

    public async Task<OrderStatus> AddAsync(OrderStatus orderStatus)
    {
        var model = orderStatus.ToModel();
        await _ordersDbContext.OrderStatuses.AddAsync(model);
        await _ordersDbContext.SaveChangesAsync();
        return model.ToDomain();
    }

    public async Task<bool> UpdateAsync(OrderStatus orderStatus)
    {
        var model = orderStatus.ToModel();
        _ordersDbContext.OrderStatuses.Attach(model);
        _ordersDbContext.Entry(model).State = EntityState.Modified;

        var affected = await _ordersDbContext.SaveChangesAsync();
        return affected > 0;
    }

    public async Task DeleteAsync(OrderStatus orderStatus)
    {
        var model = orderStatus.ToModel();
        _ordersDbContext.OrderStatuses.Attach(model);
        _ordersDbContext.OrderStatuses.Remove(model);
        await _ordersDbContext.SaveChangesAsync();
    }
}
