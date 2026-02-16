using Ogma.Domain.Catalog.Entities;

namespace Ogma.Domain.Catalog.Repositories;

public interface IItemTypeRepository
{
    /// <summary>
    /// Gets an item type by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ItemType?> GetByIdAsync(long id);

    /// <summary>
    /// Adds a new item type to the repository.
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    Task<ItemType> AddAsync(ItemType itemType);

    /// <summary>
    /// Updates an existing item type in the repository.
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    Task<bool> UpdateAsync(ItemType itemType);

    /// <summary>
    /// Deletes an item type from the repository.
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    Task DeleteAsync(ItemType itemType);

    /// <summary>
    /// Checks if an item type with the specified name exists.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<bool> ExistsByNameAsync(string name);
}
