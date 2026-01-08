using Ogma.Domain.Orders.Entities;
using System.Linq.Expressions;

namespace Ogma.Domain.Orders.Repositories;

public interface IOrderStatusRepository
{
    /// <summary>
    /// Asynchronously retrieves the order status associated with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order status to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="OrderStatus"/> if
    /// found; otherwise, <see langword="null"/>.</returns>
    Task<OrderStatus?> GetByIdAsync(long id);

    /// <summary>
    /// Asynchronously retrieves all available order statuses.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
    /// cref="OrderStatus"/> objects representing all order statuses. The collection will be empty if no order statuses
    /// are found.</returns>
    Task<IEnumerable<OrderStatus>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves all order statuses that match the specified filter criteria.
    /// </summary>
    /// <param name="predicate">An expression used to filter the order statuses. Only order statuses for which the predicate evaluates to <see
    /// langword="true"/> will be included in the result. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
    /// cref="OrderStatus"/> objects that satisfy the filter. The collection will be empty if no order statuses match
    /// the criteria.</returns>
    Task<IEnumerable<OrderStatus>> GetAllAsync(Expression<Func<OrderStatus, bool>> predicate);

    /// <summary>
    /// Asynchronously adds a new order status to the data store.
    /// </summary>
    /// <param name="orderType">The order status to add. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added order status.</returns>
    Task<OrderStatus> AddAsync(OrderStatus orderType);

    /// <summary>
    /// Asynchronously updates the status of an order to the specified value.
    /// </summary>
    /// <param name="orderType">The new status to apply to the order. Must be a valid value of <see cref="OrderStatus"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the update was
    /// successful; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync(OrderStatus orderType);

    /// <summary>
    /// Asynchronously deletes all orders with the specified status.
    /// </summary>
    /// <param name="orderType">The status of the orders to delete. Only orders matching this status will be removed.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(OrderStatus orderType);
}
