using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;
using Ogma.Infrastructure.Persistence.Orders.Contexts;
using Ogma.Infrastructure.Persistence.Orders.Extensions;

namespace Ogma.Infrastructure.Persistence.Orders.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersDbContext _ordersDbContext;
    private readonly IMapper _mapper;

    public OrderRepository(OrdersDbContext ordersDbContext, IMapper mapper)
    {
        _ordersDbContext = ordersDbContext;
        _mapper = mapper;
    }

    public async Task<Order?> GetByIdAsync(long id)
    {
        var order = await _ordersDbContext.Orders
            .Include(o => o.OrderLines)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);
        return order?.ToDomain();
    }

    public async Task<Order> AddAsync(Order order)
    {
        var model = order.ToModel();
        await _ordersDbContext.Orders.AddAsync(model);
        await _ordersDbContext.SaveChangesAsync();
        return model.ToDomain();
    }

    public async Task<bool> UpdateAsync(Order order)
    {
        var existingOrder = await _ordersDbContext.Orders
            .Include(o => o.OrderLines)
            .FirstOrDefaultAsync(o => o.Id == order.Id);

        if (existingOrder == null)
        {
            return false;
        }

        var orderModel = order.ToModel();
        _ordersDbContext.Orders.Entry(existingOrder).CurrentValues.SetValues(orderModel);
        existingOrder.OrderPartner = orderModel.OrderPartner;

        SyncOrderLines(order, existingOrder);

        var affected = await _ordersDbContext.SaveChangesAsync();
        return affected > 0;

    }

    public async Task DeleteAsync(Order order)
    {
        var model = order.ToModel();
        _ordersDbContext.Orders.Attach(model);
        _ordersDbContext.Orders.Remove(model);
        await _ordersDbContext.SaveChangesAsync();
    }

    private void SyncOrderLines(Order domainOrder, Models.Order persistenceOrder)
    {
        var persistenceOrderLinesById = persistenceOrder.OrderLines.ToDictionary(l => l.Id);

        foreach (var domainLine in domainOrder.OrderLines)
        {
            if (domainLine.Id == 0)
            {
                var newLine = _mapper.Map<Models.OrderLine>(domainLine);
                newLine.OrderId = persistenceOrder.Id;
                persistenceOrder.OrderLines.Add(newLine);
            }
            else if (persistenceOrderLinesById.TryGetValue(domainLine.Id, out var existingLine))
            {
                _mapper.Map(domainLine, existingLine);
                persistenceOrderLinesById.Remove(domainLine.Id);
            }
        }

        foreach (var orphan in persistenceOrderLinesById.Values)
        {
            persistenceOrder.OrderLines.Remove(orphan);
        }
    }

}
