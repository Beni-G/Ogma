using Ogma.Domain.Catalog.Entities;

namespace Ogma.Domain.Catalog.Repositories;

public interface ICategoryRepository
{
    /// <summary>
    /// Retrieves a category by its unique identifier without descendants.
    /// </summary>
    /// <param name="id">The unique identifier of the category.</param>
    /// <returns>The category if found; otherwise, null.</returns>
    Task<Category?> GetByIdAsync(long id);

    /// <summary>
    /// Adds a new category to the repository.
    /// </summary>
    /// <param name="category">The category to add.</param>
    Task<Category> AddAsync(Category category);

    /// <summary>
    /// Updates an existing category in the repository.
    /// </summary>
    /// <param name="category">The category to update.</param>
    Task<bool> UpdateAsync(Category category);

    /// <summary>
    /// Deletes a category from the repository.
    /// </summary>
    /// <param name="category">The category to remove.</param>
    Task DeleteAsync(Category category);
}

