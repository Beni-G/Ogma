using Ogma.Application.Catalog.Dtos;
using System.Linq.Expressions;

namespace Ogma.Application.Catalog.Ports;

public interface IItemTypeReader
{
    /// <summary>
    /// Asynchronously retrieves an item by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the item to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ItemTypeDto"/>
    /// representing the item if found; otherwise, <see langword="null"/>.</returns>
    Task<ItemTypeDto?> GetByIdAsync(long id);

    /// <summary>
    /// Asynchronously retrieves multiple items by their unique identifiers.
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<IDictionary<long, ItemTypeDto>> GetByIdsAsync(IEnumerable<long> ids);

    /// <summary>
    /// Asynchronously retrieves all items as data transfer objects.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
    /// cref="ItemTypeDto"/> objects. The collection is empty if no items are found.</returns>
    Task<IEnumerable<ItemTypeDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves all item types that satisfy the specified filter condition.
    /// </summary>
    /// <param name="predicate">An expression that defines the filter criteria to apply to the item types. Only items matching this predicate
    /// will be included in the result.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of item
    /// type DTOs that match the specified predicate. The collection is empty if no items match.</returns>
    Task<IEnumerable<ItemTypeDto>> GetAllAsync(Expression<Func<ItemTypeDto, bool>> predicate);
}
