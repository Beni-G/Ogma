using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ogma.Infrastructure.Persistence.Orders.Repositories;

public class OrderStatusRepository : IOrderStatusRepository
{
    public Task<OrderStatus> AddAsync(OrderStatus orderType)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(OrderStatus orderType)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<OrderStatus>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<OrderStatus>> GetAllAsync(Expression<Func<OrderStatus, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<OrderStatus?> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(OrderStatus orderType)
    {
        throw new NotImplementedException();
    }
}
