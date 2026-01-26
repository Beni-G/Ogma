using Ogma.Domain.Orders.Entities;

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
    /// Asynchronously adds a new order status to the data store.
    /// </summary>
    /// <param name="orderType">The order status to add. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added order status.</returns>
    Task<OrderStatus> AddAsync(OrderStatus orderStatus);

    /// <summary>
    /// Asynchronously updates the status of an order to the specified value.
    /// </summary>
    /// <param name="orderType">The new status to apply to the order. Must be a valid value of <see cref="OrderStatus"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the update was
    /// successful; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync(OrderStatus orderStatus);

    /// <summary>
    /// Asynchronously deletes all orders with the specified status.
    /// </summary>
    /// <param name="orderType">The status of the orders to delete. Only orders matching this status will be removed.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(OrderStatus orderStatus);
}
