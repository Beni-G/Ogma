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
