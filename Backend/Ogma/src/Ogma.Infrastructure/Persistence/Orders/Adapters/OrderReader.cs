using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;
using Ogma.Infrastructure.Persistence.Orders.Contexts;
using Ogma.Infrastructure.Persistence.Orders.Extensions;
using Ogma.Infrastructure.Persistence.Orders.Models;
using System.Linq.Expressions;

namespace Ogma.Infrastructure.Persistence.Orders.Adapters;

public class OrderReader : IOrderReader
{
    private readonly OrdersDbContext _ordersDbContext;
    private readonly IMapper _mapper;

    public OrderReader(OrdersDbContext ordersDbContext, IMapper mapper)
    {
        _ordersDbContext = ordersDbContext;
        _mapper = mapper;
    }

    public async Task<OrderDto?> GetByIdAsync(long id)
    {
        var order = await _ordersDbContext.Orders
            .Include(o => o.OrderType)
            .Include(o => o.OrderStatus)
            .Include(o => o.OrderLines)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id);
        return order?.ToDto();
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        return await _ordersDbContext.Orders
            .Include(o => o.OrderType)
            .Include(o => o.OrderStatus)
            .Include(o => o.OrderLines)
            .AsNoTracking()
            .Select(o => o.ToDto())
            .ToListAsync();
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync(Expression<Func<OrderDto, bool>> predicate)
    {
        var modelPredicate = _mapper.MapExpression<Expression<Func<Order, bool>>>(predicate);

        return await _ordersDbContext.Orders
            .Include(o => o.OrderType)
            .Include(o => o.OrderStatus)
            .Include(o => o.OrderLines)
            .AsNoTracking()
            .Where(modelPredicate)
            .Select(o => o.ToDto())
            .ToListAsync();
    }
}
