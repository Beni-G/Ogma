using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Orders.Entities;

public class Order : AggregateRoot<long>
{
    private List<OrderLine> _orderLines = new();

    public long PartnerId { get; private set; }
    public string OrderNumber { get; private set; }
    public DateTime OrderDate { get; private set; }
    public long OrderTypeId { get; private set; }
    public long OrderStatusId { get; private set; }
    public string? AdditionalInformation { get; private set; }
    public IReadOnlyCollection<OrderLine> OrderLines => _orderLines.AsReadOnly();

    private Order(long partnerId, string orderNumber, DateTime orderDate, long orderTypeId, long orderStatusId, string? additionalInformation = "")
    {
        if (partnerId <= 0)
        {
            throw new ArgumentException("Partner ID must be a positive number.", nameof(partnerId));
        }
        if (string.IsNullOrWhiteSpace(orderNumber))
        {
            throw new ArgumentException(nameof(orderNumber));
        }
        if (orderDate == default)
        {
            throw new ArgumentException("Order date must be a valid date.", nameof(orderDate));
        }
        if (orderTypeId <= 0)
        {
            throw new ArgumentException("Order Type ID must be a positive number.", nameof(orderTypeId));
        }
        if (orderStatusId <= 0)
        {
            throw new ArgumentException("Order Status ID must be a positive number.", nameof(orderStatusId));
        }

        PartnerId = partnerId;
        OrderNumber = orderNumber;
        OrderDate = orderDate;
        OrderTypeId = orderTypeId;
        OrderStatusId = orderStatusId;
        AdditionalInformation = additionalInformation;
    }

    private Order(long id, long partnerId, string orderNumber, DateTime orderDate, long orderTypeId, long orderStatusId, string? additionalInformation = "")
    {
        if (id <= 0)
        {
            throw new ArgumentException("ID must be a positive number.", nameof(id));
        }
        if (partnerId <= 0)
        {
            throw new ArgumentException("Partner ID must be a positive number.", nameof(partnerId));
        }
        if (string.IsNullOrWhiteSpace(orderNumber))
        {
            throw new ArgumentException(nameof(orderNumber));
        }
        if (orderDate == default)
        {
            throw new ArgumentException("Order date must be a valid date.", nameof(orderDate));
        }
        if (orderTypeId <= 0)
        {
            throw new ArgumentException("Order Type ID must be a positive number.", nameof(orderTypeId));
        }
        if (orderStatusId <= 0)
        {
            throw new ArgumentException("Order Status ID must be a positive number.", nameof(orderStatusId));
        }

        Id = id;
        PartnerId = partnerId;
        OrderNumber = orderNumber;
        OrderDate = orderDate;
        OrderTypeId = orderTypeId;
        OrderStatusId = orderStatusId;
        AdditionalInformation = additionalInformation;
    }

    /// <summary>
    /// Creates a new instance of the Order class with the specified partner, order number, date, type, status, and
    /// optional additional information.
    /// </summary>
    /// <param name="partnerId">The unique identifier of the partner associated with the order.</param>
    /// <param name="orderNumber">The external or business order number that uniquely identifies the order within the partner's system. Cannot be
    /// null.</param>
    /// <param name="orderDate">The date and time when the order was placed.</param>
    /// <param name="orderTypeId">The identifier of the order type to categorize the order.</param>
    /// <param name="orderStatusId">The identifier representing the current status of the order.</param>
    /// <param name="additionalInformation">Optional additional information or notes about the order. Can be null or empty.</param>
    /// <returns>A new Order instance initialized with the provided values.</returns>
    public static Order Create(long partnerId, string orderNumber, DateTime orderDate, long orderTypeId, long orderStatusId, string? additionalInformation = "")
        => new(partnerId, orderNumber, orderDate, orderTypeId, orderStatusId, additionalInformation);

    /// <summary>
    /// Recreates an Order instance from the specified persisted values.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <param name="partnerId">The unique identifier of the partner associated with the order.</param>
    /// <param name="orderNumber">The external or business order number that uniquely identifies the order within the partner's system.</param>
    /// <param name="orderDate">The date and time when the order was placed.</param>
    /// <param name="orderTypeId">The identifier representing the type of the order.</param>
    /// <param name="orderStatusId">The identifier representing the current status of the order.</param>
    /// <param name="additionalInformation">Optional additional information or notes related to the order. Can be null or empty.</param>
    /// <returns>An Order instance populated with the provided values.</returns>
    public static Order Reconstitute(long id, long partnerId, string orderNumber, DateTime orderDate, long orderTypeId, long orderStatusId, string? additionalInformation = "")
        => new(id, partnerId, orderNumber, orderDate, orderTypeId, orderStatusId, additionalInformation);

    /// <summary>
    /// Adds the specified order line to the order.
    /// </summary>
    /// <remarks>All order lines in the order must use the same conversion currency. The first order line
    /// added determines the required currency for subsequent order lines.</remarks>
    /// <param name="orderLine">The order line to add. Must not be null and must have a unique item ID within the order.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="orderLine"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if an order line with the same item ID already exists in the order, or if the conversion currency of the
    /// order line does not match the existing order lines.</exception>
    public void AddOrderLine(OrderLine orderLine)
    {
        if (orderLine == null)
        {
            throw new ArgumentNullException(nameof(orderLine));
        }
        if (_orderLines.Any(ol => ol.ItemId == orderLine.ItemId))
        {
            throw new InvalidOperationException($"An order line with Item ID {orderLine.ItemId} already exists in the order.");
        }

        string? conversionCurrency = _orderLines.Count == 0
            ? null
            : _orderLines.First().ConvertedPrice.Currency!;
        if (conversionCurrency != null && orderLine.ConvertedPrice.Currency != conversionCurrency)
        {
            throw new InvalidOperationException($"All order lines must have the same conversion currency. Expected: {conversionCurrency}, but got: {orderLine.ConvertedPrice.Currency}");
        }

        _orderLines.Add(orderLine);
    }

    /// <summary>
    /// Removes the order line with the specified identifier from the order.
    /// </summary>
    /// <param name="orderLineId">The unique identifier of the order line to remove.</param>
    /// <exception cref="InvalidOperationException">Thrown if no order line with the specified identifier exists in the order.</exception>
    public void RemoveOrderLine(long orderLineId)
    {
        var orderLine = _orderLines.FirstOrDefault(ol => ol.Id == orderLineId);
        if (orderLine == null)
        {
            throw new InvalidOperationException($"No order line with ID {orderLineId} exists in the order.");
        }

        _orderLines.Remove(orderLine);
    }

    /// <summary>
    /// Removes all order lines from the current order.
    /// </summary>
    /// <remarks>After calling this method, the order will contain no order lines. This operation cannot be
    /// undone.</remarks>
    public void ClearOrderLines() => _orderLines.Clear();

    /// <summary>
    /// Updates the order details with the specified values.
    /// </summary>
    /// <param name="partnerId">The unique identifier of the partner associated with the order. Must be a positive number.</param>
    /// <param name="orderNumber">The order number to assign. Cannot be null, empty, or consist only of white-space characters.</param>
    /// <param name="orderDate">The date of the order. Must be a valid, non-default date value.</param>
    /// <param name="orderTypeId">The identifier of the order type. Must be a positive number.</param>
    /// <param name="orderStatusId">The identifier of the order status. Must be a positive number.</param>
    /// <param name="additionalInformation">Additional information to associate with the order. Can be null or empty if no extra information is required.</param>
    /// <exception cref="ArgumentException">Thrown when any parameter is invalid: <paramref name="partnerId"/> is not positive; <paramref
    /// name="orderNumber"/> is null, empty, or white space; <paramref name="orderDate"/> is the default value;
    /// <paramref name="orderTypeId"/> or <paramref name="orderStatusId"/> is not positive.</exception>
    public void Update(long partnerId, string orderNumber, DateTime orderDate, long orderTypeId, long orderStatusId, string additionalInformation)
    {
        if (partnerId <= 0)
        {
            throw new ArgumentException("Partner ID must be a positive number.", nameof(partnerId));
        }
        if (string.IsNullOrWhiteSpace(orderNumber))
        {
            throw new ArgumentException(nameof(orderNumber));
        }
        if (orderDate == default)
        {
            throw new ArgumentException("Order date must be a valid date.", nameof(orderDate));
        }
        if (orderTypeId <= 0)
        {
            throw new ArgumentException("Order Type ID must be a positive number.", nameof(orderTypeId));
        }
        if (orderStatusId <= 0)
        {
            throw new ArgumentException("Order Status ID must be a positive number.", nameof(orderStatusId));
        }

        PartnerId = partnerId;
        OrderNumber = orderNumber;
        OrderDate = orderDate;
        OrderTypeId = orderTypeId;
        OrderStatusId = orderStatusId;
        AdditionalInformation = additionalInformation;

        Touch();
    }

    /// <summary>
    /// Calculates the total converted amount for all order lines, or returns null if there are no order lines.
    /// </summary>
    /// <remarks>If the order contains order lines with different currencies, the currency of the first order
    /// line is used for the total. Ensure all order lines use the same currency to avoid inconsistent
    /// results.</remarks>
    /// <returns>A <see cref="Money"/> object representing the sum of the converted values of all order lines, using the currency
    /// of the first order line; or <see langword="null"/> if there are no order lines.</returns>
    public Money? GetTotalConvertedAmount()
        => _orderLines.Count == 0
            ? null
            : new Money(_orderLines.Sum(ol => ol.LineActiveConvertedValue.Amount), _orderLines.FirstOrDefault()?.LineActiveConvertedValue.Currency!);

}
