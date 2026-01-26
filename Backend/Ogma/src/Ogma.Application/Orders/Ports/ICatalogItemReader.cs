using Ogma.Application.Orders.Dtos;

namespace Ogma.Application.Orders.Ports;

public interface ICatalogItemReader
{
    /// <summary>
    /// Retrieves a catalog item by its identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<OrderItemDto?> GetByIdAsync(long id);

    /// <summary>
    /// Retrieves multiple catalog items by their identifiers.
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<IDictionary<long, OrderItemDto>> GetByIdsAsync(IEnumerable<long> ids);
}
