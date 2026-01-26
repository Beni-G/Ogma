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

public class OrderStatusReader : IOrderStatusReader
{
    private readonly OrdersDbContext _ordersDbContext;
    private readonly IMapper _mapper;
    public OrderStatusReader(OrdersDbContext ordersDbContext, IMapper mapper)
    {
        _ordersDbContext = ordersDbContext;
        _mapper = mapper;
    }

    public async Task<OrderStatusDto?> GetByIdAsync(long id)
    {
        var orderStatus = await _ordersDbContext.OrderStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(os => os.Id == id);
        return orderStatus?.ToDto();
    }

    public async Task<IEnumerable<OrderStatusDto>> GetAllAsync()
    {
        return await _ordersDbContext.OrderStatuses
            .AsNoTracking()
            .Select(os => os.ToDto())
            .ToListAsync();
    }

    public async Task<IEnumerable<OrderStatusDto>> GetAllAsync(Expression<Func<OrderStatusDto, bool>> predicate)
    {
        var modelPredicate = _mapper.MapExpression<Expression<Func<OrderStatus, bool>>>(predicate);

        return await _ordersDbContext.OrderStatuses
            .AsNoTracking()
            .Where(modelPredicate)
            .Select(os => os.ToDto())
            .ToListAsync();
    }
}
