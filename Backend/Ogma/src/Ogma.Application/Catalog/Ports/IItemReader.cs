using Ogma.Application.Catalog.Dtos;
using System.Linq.Expressions;

namespace Ogma.Application.Catalog.Ports;

public interface IItemReader
{
    /// <summary>
    /// Asynchronously retrieves an item by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the item to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ItemDto"/> if the item
    /// is found; otherwise, <see langword="null"/>.</returns>
    Task<ItemDto?> GetByIdAsync(long id);

    /// <summary>
    /// Asynchronously retrieves multiple items by their unique identifiers.
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<IDictionary<long, ItemDto>> GetByIdsAsync(IEnumerable<long> ids);

    /// <summary>
    /// Asynchronously retrieves all items.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
    /// cref="ItemDto"/> objects representing all items. The collection will be empty if no items are found.</returns>
    Task<IEnumerable<ItemDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves all items that satisfy the specified filter condition.
    /// </summary>
    /// <param name="predicate">An expression that defines the filter to apply to the items. Only items for which the predicate evaluates to
    /// true are included in the result. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of items
    /// matching the filter. The collection is empty if no items match.</returns>
    Task<IEnumerable<ItemDto>> GetAllAsync(Expression<Func<ItemDto, bool>> predicate);
}
