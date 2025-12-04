using Ogma.Domain.Partners.Entities;
using System.Linq.Expressions;

namespace Ogma.Domain.Partners.Repositories;

public interface IPartnerRoleTypeRepository
{
    /// <summary>
    /// Asynchronously retrieves the partner role type associated with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the partner role type to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the partner role type if found;
    /// otherwise, null.</returns>
    Task<PartnerRoleType?> GetByIdAsync(long id);

    /// <summary>
    /// Asynchronously retrieves all available partner role types.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
    /// cref="PartnerRoleType"/> objects representing all partner role types. The collection will be empty if no partner
    /// role types are available.</returns>
    Task<IEnumerable<PartnerRoleType>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves all partner role types that satisfy the specified filter criteria.
    /// </summary>
    /// <param name="predicate">An expression used to filter the partner role types. Only items for which the predicate evaluates to <see
    /// langword="true"/> are included in the result. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of partner
    /// role types matching the filter. If no items match, the collection will be empty.</returns>
    Task<IEnumerable<PartnerRoleType>> GetAllAsync(Expression<Func<PartnerRoleType, bool>> predicate);

    /// <summary>
    /// Asynchronously adds a new partner role type to the data store.
    /// </summary>
    /// <param name="partnerRoleType">The partner role type to add. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added partner role type,
    /// including any updates made during insertion.</returns>
    Task<PartnerRoleType> AddAsync(PartnerRoleType partnerRoleType);

    /// <summary>
    /// Asynchronously updates the specified partner role type in the data store.
    /// </summary>
    /// <param name="partnerRoleType">The partner role type to update. Must not be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the update was
    /// successful; otherwise, <see langword="false"/>.</returns>
    Task<bool> UpdateAsync(PartnerRoleType partnerRoleType);

    /// <summary>
    /// Asynchronously deletes the specified partner role type from the data store.
    /// </summary>
    /// <param name="partnerRoleType">The partner role type to delete. Must be a valid value of <see cref="PartnerRoleType"/>.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(PartnerRoleType partnerRoleType);

}
