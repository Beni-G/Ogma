using Ogma.Domain.Catalog.Entities;
using System.Linq.Expressions;

namespace Ogma.Domain.Catalog.Repositories;
public interface IItemRepository
{
    /// <summary>
    /// Gets an item by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Item?> GetByIdAsync(long id);

    /// <summary>
    /// Gets all items.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Item>> GetAllAsync();

    /// <summary>
    /// Gets all items that match the specified predicate.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<IEnumerable<Item>> GetAllAsync(Expression<Func<Item, bool>> predicate);

    /// <summary>
    /// Adds a new item to the repository.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task AddAsync(Item item);

    /// <summary>
    /// Updates an existing item in the repository.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task UpdateAsync(Item item);

    /// <summary>
    /// Deletes an item from the repository.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task DeleteAsync(Item item);

    /// <summary>
    /// Checks if an item with the specified code exists.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<bool> ExistsByCodeAsync(string code);
}
