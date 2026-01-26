using Ogma.Application.Orders.Dtos;

namespace Ogma.Application.Orders.Ports;

public interface IPartnerReader
{
    /// <summary>
    /// Retrieves a partner by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<OrderPartnerDto?> GetByIdAsync(long id);

    /// <summary>
    /// Retrieves multiple partners by their unique identifiers.
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<IDictionary<long, OrderPartnerDto>> GetByIdsAsync(IEnumerable<long> ids);
}
