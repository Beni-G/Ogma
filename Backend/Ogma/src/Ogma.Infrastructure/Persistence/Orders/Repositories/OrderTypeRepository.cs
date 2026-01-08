using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ogma.Infrastructure.Persistence.Orders.Repositories;

public class OrderTypeRepository : IOrderTypeRepository
{
    public Task<OrderType> AddAsync(OrderType orderType)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(OrderType orderType)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<OrderType>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<OrderType>> GetAllAsync(Expression<Func<OrderType, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<OrderType?> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(OrderType orderType)
    {
        throw new NotImplementedException();
    }
}
