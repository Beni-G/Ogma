using Ogma.Domain.Partners.Entities;
using System.Linq.Expressions;

namespace Ogma.Domain.Partners.Repositories;

public interface IPartnerRepository
{
    /// <summary>
    /// Asynchronously retrieves a partner entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the partner to retrieve. Must be greater than zero.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the partner entity if found;
    /// otherwise, null.</returns>
    Task<Partner?> GetByIdAsync(int id);

    /// <summary>
    /// Asynchronously retrieves all partners from the data source.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
    /// cref="Partner"/> objects representing all partners. The collection will be empty if no partners are found.</returns>
    Task<IEnumerable<Partner>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves all partners that satisfy the specified filter criteria.
    /// </summary>
    /// <param name="predicate">An expression used to filter the partners to be retrieved. Only partners for which the predicate evaluates to
    /// <see langword="true"/> will be included in the result. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of partners
    /// matching the filter. If no partners match, the collection will be empty.</returns>
    Task<IEnumerable<Partner>> GetAllAsync(Expression<Func<Partner, bool>> predicate);

    /// <summary>
    /// Asynchronously adds a new partner to the system.
    /// </summary>
    /// <param name="partner">The partner entity to add. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added partner entity, including
    /// any updated properties set by the system.</returns>
    Task<Partner> AddAsync(Partner partner);

    /// <summary>
    /// Asynchronously updates the specified partner entity in the data store.
    /// </summary>
    /// <param name="partner">The partner entity to update. Cannot be null. The entity must have a valid identifier.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the update was
    /// successful; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync(Partner partner);

    /// <summary>
    /// Asynchronously deletes the specified partner from the system.
    /// </summary>
    /// <param name="partner">The partner entity to delete. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Partner partner);
}
