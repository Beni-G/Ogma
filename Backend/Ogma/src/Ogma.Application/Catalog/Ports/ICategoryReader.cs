using Ogma.Application.Catalog.Dtos;
using System.Linq.Expressions;

namespace Ogma.Application.Catalog.Ports;

public interface ICategoryReader
{
    /// <summary>
    /// Asynchronously retrieves a category by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the category to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="CategoryDto"/>
    /// representing the category if found; otherwise, <see langword="null"/>.</returns>
    Task<CategoryDto?> GetByIdAsync(long id);

    /// <summary>
    /// Asynchronously retrieves multiple categories by their unique identifiers.
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<IDictionary<long, CategoryDto>> GetByIdsAsync(IEnumerable<long> ids);

    /// <summary>
    /// Asynchronously retrieves all categories.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
    /// cref="CategoryDto"/> objects representing all categories. The collection will be empty if no categories are
    /// found.</returns>
    Task<IEnumerable<CategoryDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves all category data transfer objects that satisfy the specified filter condition.
    /// </summary>
    /// <param name="predicate">An expression that defines the filter criteria to apply to the categories. Only categories for which the
    /// predicate evaluates to true are included in the result. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of
    /// CategoryDto objects that match the specified predicate. The collection is empty if no categories match.</returns>
    Task<IEnumerable<CategoryDto>> GetAllAsync(Expression<Func<CategoryDto, bool>> predicate);

    /// <summary>
    /// Asynchronously retrieves all categories organized in a hierarchical tree structure, including each category's
    /// descendants.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of category data
    /// transfer objects, each including its descendant categories. The collection is empty if no categories are found.</returns>
    Task<IEnumerable<CategoryDto>> GetAllCategoriesTreeAsync();

    /// <summary>
    /// Asynchronously retrieves the child categories of the specified parent category.
    /// </summary>
    /// <param name="parentId">The unique identifier of the parent category for which to retrieve child categories.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
    /// cref="CategoryDto"/> objects representing the child categories. The collection is empty if the parent category
    /// has no children.</returns>
    Task<IEnumerable<CategoryDto>> GetChildrenAsync(long parentId);

    /// <summary>
    /// Asynchronously retrieves all descendant categories of the specified root category.
    /// </summary>
    /// <param name="rootId">The unique identifier of the root category for which to retrieve all descendants.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
    /// cref="CategoryDto"/> objects representing all descendant categories of the specified root. The collection is
    /// empty if the root category has no descendants.</returns>
    Task<IEnumerable<CategoryDto>> GetDescendantsAsync(long rootId);

    /// <summary>
    /// Asynchronously retrieves all ancestor categories of the specified category, starting from its immediate parent
    /// up to the root.
    /// </summary>
    /// <param name="leafId">The unique identifier of the category for which to retrieve ancestor categories. Must be a valid category ID.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of ancestor categories,
    /// ordered from the immediate parent to the root. Returns an empty collection if the specified category has no
    /// ancestors.</returns>
    Task<IEnumerable<CategoryDto>> GetAncestorsAsync(long leafId);

    /// <summary>
    /// Asynchronously retrieves all ancestor categories for each of the specified leaf category IDs.
    /// </summary>
    /// <param name="leafIds"></param>
    /// <returns></returns>
    Task<IDictionary<long, IEnumerable<CategoryDto>>> GetAncestorsAsync(IEnumerable<long> leafIds);
}
