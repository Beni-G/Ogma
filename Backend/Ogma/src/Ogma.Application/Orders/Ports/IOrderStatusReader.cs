using Ogma.Application.Orders.Dtos;
using System.Linq.Expressions;

namespace Ogma.Application.Orders.Ports;

public interface IOrderStatusReader
{
    /// <summary>
    /// Asynchronously retrieves the order status for the specified order identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order whose status is to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="OrderStatusDto"/> with
    /// the order status if found; otherwise, <see langword="null"/>.</returns>
    Task<OrderStatusDto?> GetByIdAsync(long id);

    /// <summary>
    /// Asynchronously retrieves all available order status records.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
    /// cref="OrderStatusDto"/> objects representing all order statuses. The collection will be empty if no order
    /// statuses are found.</returns>
    Task<IEnumerable<OrderStatusDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves all order statuses for orders that match the specified predicate.
    /// </summary>
    /// <param name="predicate">An expression used to filter orders. Only orders for which the predicate evaluates to <see langword="true"/>
    /// will be included in the results. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
    /// cref="OrderStatusDto"/> objects corresponding to the matching orders. The collection will be empty if no orders
    /// match the predicate.</returns>
    Task<IEnumerable<OrderStatusDto>> GetAllAsync(Expression<Func<OrderStatusDto, bool>> predicate);
}
