using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ogma.Infrastructure.Persistence.Orders.Repositories;

public class OrderRepository : IOrderRepository
{
    public Task<Order> AddAsync(Order orderType)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Order orderType)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Order>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Order>> GetAllAsync(Expression<Func<Order, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<Order?> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Order orderType)
    {
        throw new NotImplementedException();
    }
}
