using Ogma.Application.Orders.Dtos;
using System.Linq.Expressions;

namespace Ogma.Application.Orders.Ports;

public interface IOrderReader
{
    /// <summary>
    /// Asynchronously retrieves the order with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="OrderDto"/>
    /// representing the order if found; otherwise, <see langword="null"/>.</returns>
    Task<OrderDto?> GetByIdAsync(long id);

    /// <summary>
    /// Asynchronously retrieves all orders.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
    /// cref="OrderDto"/> objects representing all orders. If no orders exist, the collection will be empty.</returns>
    Task<IEnumerable<OrderDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves all orders that satisfy the specified filter criteria.
    /// </summary>
    /// <param name="predicate">An expression used to filter the orders to be retrieved. Only orders for which the predicate evaluates to <see
    /// langword="true"/> will be included in the result. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
    /// cref="OrderDto"/> objects matching the filter. If no orders match, the collection will be empty.</returns>
    Task<IEnumerable<OrderDto>> GetAllAsync(Expression<Func<OrderDto, bool>> predicate);
}
