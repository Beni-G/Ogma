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
    Task<Partner?> GetByIdAsync(long id);

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
