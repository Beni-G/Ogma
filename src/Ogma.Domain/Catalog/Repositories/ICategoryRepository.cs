using Ogma.Domain.Catalog.Entities;
using System.Linq.Expressions;

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
    /// Retrieves all categories without descendants.
    /// </summary>
    /// <returns>A collection of all categories.</returns>
    Task<IEnumerable<Category>> GetAllAsync();

    /// <summary>
    /// Retrieves all categories that match the given predicate without descendants.
    /// </summary>
    /// <param name="predicate">A filter expression to apply.</param>
    /// <returns>A collection of matching categories.</returns>
    Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>> predicate);

    /// <summary>
    /// Retrieves entire categories tree.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Category>> GetAllCategoriesTreeAsync();

    /// <summary>
    /// Retrieves the immediate children of a given category.
    /// </summary>
    /// <param name="parentId">The ID of the parent category.</param>
    /// <returns>A collection of child categories.</returns>
    Task<IEnumerable<Category>> GetChildrenAsync(long parentId);

    /// <summary>
    /// Retrieves all descendants of a given category.
    /// </summary>
    /// <param name="rootId">The ID of the root category.</param>
    /// <returns>A collection of descendant categories.</returns>
    Task<IEnumerable<Category>> GetDescendantsAsync(long rootId);

    /// <summary>
    /// Retrieves all ancestors of a given category.
    /// </summary>
    /// <param name="leafId">The ID of the leaf category.</param>
    /// <returns>A collection of ancestor categories.</returns>
    Task<IEnumerable<Category>> GetAncestorsAsync(long leafId);

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

