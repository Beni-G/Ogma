using Ogma.Application.Orders.Dtos;
using System.Linq.Expressions;

namespace Ogma.Application.Orders.Ports;

public interface IOrderTypeReader
{
    /// <summary>
    /// Asynchronously retrieves the order type with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order type to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="OrderTypeDto"/> if an
    /// order type with the specified identifier exists; otherwise, <see langword="null"/>.</returns>
    Task<OrderTypeDto?> GetByIdAsync(long id);

    /// <summary>
    /// Asynchronously retrieves all available order types.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
    /// cref="OrderTypeDto"/> objects representing all order types. The collection will be empty if no order types are
    /// found.</returns>
    Task<IEnumerable<OrderTypeDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves all order types that satisfy the specified filter criteria.
    /// </summary>
    /// <param name="predicate">An expression used to filter the order types. Only order types for which the predicate evaluates to <see
    /// langword="true"/> will be included in the result. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
    /// cref="OrderTypeDto"/> objects matching the filter. The collection will be empty if no order types match the
    /// criteria.</returns>
    Task<IEnumerable<OrderTypeDto>> GetAllAsync(Expression<Func<OrderTypeDto, bool>> predicate);
}
