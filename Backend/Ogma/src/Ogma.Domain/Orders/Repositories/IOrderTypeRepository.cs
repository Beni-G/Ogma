using Ogma.Domain.Orders.Entities;
using System.Linq.Expressions;

namespace Ogma.Domain.Orders.Repositories;

public interface IOrderTypeRepository
{
    /// <summary>
    /// Asynchronously retrieves the order with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the order if found; otherwise, null.</returns>
    Task<OrderType?> GetByIdAsync(long id);

    /// <summary>
    /// Asynchronously retrieves all available order types.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of all
    /// order types.</returns>
    Task<IEnumerable<OrderType>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves all order types that satisfy the specified filter criteria.
    /// </summary>
    /// <param name="predicate">An expression used to filter the order types. Only order types for which the predicate evaluates to <see
    /// langword="true"/> are included in the result. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of order
    /// types matching the filter. If no order types match, the collection is empty.</returns>
    Task<IEnumerable<OrderType>> GetAllAsync(Expression<Func<OrderType, bool>> predicate);

    /// <summary>
    /// Asynchronously adds a new order type to the system.
    /// </summary>
    /// <param name="orderType">The order type to add. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added order type, including any
    /// updates made during the add operation.</returns>
    Task<OrderType> AddAsync(OrderType orderType);

    /// <summary>
    /// Asynchronously updates the specified order type in the data store.
    /// </summary>
    /// <param name="orderType">The order type to update. Must be a valid, existing order type.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the update was
    /// successful; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync(OrderType orderType);

    /// <summary>
    /// Asynchronously deletes all orders of the specified type.
    /// </summary>
    /// <param name="orderType">The type of orders to delete. Only orders matching this type will be removed.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(OrderType orderType);
}
