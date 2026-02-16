using Ogma.Application.Partners.Dtos;
using System.Linq.Expressions;

namespace Ogma.Application.Partners.Ports;

public interface IPartnerReader
{
    /// <summary>
    /// Asynchronously retrieves a partner by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<PartnerDto?> GetByIdAsync(long id);

    /// <summary>
    /// Asynchronously retrieves multiple partners by their unique identifiers.
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<IDictionary<long, PartnerDto>> GetByIdsAsync(IEnumerable<long> ids);

    /// <summary>
    /// Asynchronously retrieves all partners.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<PartnerDto>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves partners that satisfy a specified condition defined by the given predicate.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<IEnumerable<PartnerDto>> GetAllAsync(Expression<Func<PartnerDto, bool>> predicate);
}
