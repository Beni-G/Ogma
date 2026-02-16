using Ogma.Application.Partners.Dtos;
using System.Linq.Expressions;

namespace Ogma.Application.Partners.Ports;

public interface IPartnerRoleTypeReader
{
    /// <summary>
    /// Asynchronously retrieves a partner role type by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<PartnerRoleTypeDto?> GetByIdAsync(long id);

    /// <summary>
    /// Asynchronously retrieves multiple partner role types based on a collection of unique identifiers.
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<IDictionary<long, PartnerRoleTypeDto>> GetByIdsAsync(IEnumerable<long> ids);

    /// <summary>
    /// Asynchronously retrieves all partner role types.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<PartnerRoleTypeDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves all partner role types that satisfy the specified predicate.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<IEnumerable<PartnerRoleTypeDto>> GetAllAsync(Expression<Func<PartnerRoleTypeDto, bool>> predicate);
}
