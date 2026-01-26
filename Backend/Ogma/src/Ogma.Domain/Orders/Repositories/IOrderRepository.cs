using Ogma.Domain.Orders.Entities;

namespace Ogma.Domain.Orders.Repositories;

public interface IOrderRepository
{
    /// <summary>
    /// Asynchronously retrieves the order with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Order"/> if found;
    /// otherwise, <see langword="null"/>.</returns>
    Task<Order?> GetByIdAsync(long id);

    /// <summary>
    /// Asynchronously adds a new order to the system.
    /// </summary>
    /// <param name="orderType">The order to add. Must not be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added order, including any
    /// updates made during creation.</returns>
    Task<Order> AddAsync(Order orderType);

    /// <summary>
    /// Asynchronously updates the specified order in the data store.
    /// </summary>
    /// <param name="orderType">The order to update. Cannot be null. The order's identifier must correspond to an existing order in the data
    /// store.</param>
    /// <returns>A task that represents the asynchronous update operation. The task result is <see langword="true"/> if the
    /// update was successful; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync(Order orderType);

    /// <summary>
    /// Asynchronously deletes the specified order from the data store.
    /// </summary>
    /// <param name="orderType">The order to delete. Must not be null.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Order orderType);
}
