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

public class OrderTypeReader : IOrderTypeReader
{
    private readonly OrdersDbContext _ordersDbContext;
    private readonly IMapper _mapper;

    public OrderTypeReader(OrdersDbContext ordersDbContext, IMapper mapper)
    {
        _ordersDbContext = ordersDbContext;
        _mapper = mapper;
    }

    public async Task<OrderTypeDto?> GetByIdAsync(long id)
    {
        var orderType = await _ordersDbContext.OrderTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(ot => ot.Id == id);
        return orderType?.ToDto();
    }

    public async Task<IEnumerable<OrderTypeDto>> GetAllAsync()
    {
        return await _ordersDbContext.OrderTypes
            .AsNoTracking()
            .Select(ot => ot.ToDto())
            .ToListAsync();
    }

    public async Task<IEnumerable<OrderTypeDto>> GetAllAsync(Expression<Func<OrderTypeDto, bool>> predicate)
    {
        var modelPredicate = _mapper.MapExpression<Expression<Func<OrderType, bool>>>(predicate);

        return await _ordersDbContext.OrderTypes
            .AsNoTracking()
            .Where(modelPredicate)
            .Select(ot => ot.ToDto())
            .ToListAsync();
    }
}
