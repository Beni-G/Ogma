using Ogma.Domain.Orders.ValueObjects;
using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Orders.Entities;

public class Order : AggregateRoot<long>
{
    private List<OrderLine> _orderLines = new();

    public OrderPartner OrderPartner { get; private set; }
    public string OrderNumber { get; private set; }
    public DateTime OrderDate { get; private set; }
    public long OrderTypeId { get; private set; }
    public long OrderStatusId { get; private set; }
    public string? AdditionalInformation { get; private set; }
    public IReadOnlyCollection<OrderLine> OrderLines => _orderLines.AsReadOnly();

    private Order(OrderPartner orderPartner, string orderNumber, DateTime orderDate, long orderTypeId, long orderStatusId, string? additionalInformation = "")
    {
        if (orderPartner == null)
        {
            throw new ArgumentNullException(nameof(orderPartner));
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

        OrderPartner = orderPartner;
        OrderNumber = orderNumber;
        OrderDate = orderDate;
        OrderTypeId = orderTypeId;
        OrderStatusId = orderStatusId;
        AdditionalInformation = additionalInformation;
    }

    private Order(
        long id,
        OrderPartner orderPartner,
        string orderNumber,
        DateTime orderDate,
        long orderTypeId,
        long orderStatusId,
        EntityMetadata metadata,
        string? additionalInformation = "") : base(id, metadata)
    {
        if (orderPartner == null!)
        {
            throw new ArgumentNullException(nameof(orderPartner));
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
        OrderPartner = orderPartner;
        OrderNumber = orderNumber;
        OrderDate = orderDate;
        OrderTypeId = orderTypeId;
        OrderStatusId = orderStatusId;
        AdditionalInformation = additionalInformation;
    }

    /// <summary>
    /// Creates a new Order instance with the specified details.
    /// </summary>
    /// <param name="orderPartner"></param>
    /// <param name="orderNumber"></param>
    /// <param name="orderDate"></param>
    /// <param name="orderTypeId"></param>
    /// <param name="orderStatusId"></param>
    /// <param name="additionalInformation"></param>
    /// <returns></returns>
    public static Order Create(OrderPartner orderPartner, string orderNumber, DateTime orderDate, long orderTypeId, long orderStatusId, string? additionalInformation = "")
        => new(orderPartner, orderNumber, orderDate, orderTypeId, orderStatusId, additionalInformation);

    /// <summary>
    /// Reconstitutes an Order instance from persisted data.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="orderPartner"></param>
    /// <param name="orderNumber"></param>
    /// <param name="orderDate"></param>
    /// <param name="orderTypeId"></param>
    /// <param name="orderStatusId"></param>
    /// <param name="metadata"></param>
    /// <param name="additionalInformation"></param>
    /// <param name="orderLines"></param>
    /// <returns></returns>
    public static Order Reconstitute(
        long id,
        OrderPartner orderPartner,
        string orderNumber,
        DateTime orderDate,
        long orderTypeId,
        long orderStatusId,
        EntityMetadata metadata,
        string? additionalInformation = "",
        IReadOnlyCollection<OrderLine> orderLines = null!)
    {
        var order = new Order(id, orderPartner, orderNumber, orderDate, orderTypeId, orderStatusId, metadata, additionalInformation);
        if (orderLines != null)
        {
            foreach (var orderLine in orderLines)
            {
                order._orderLines.Add(orderLine);
            }
        }
        return order;
    }

    /// <summary>
    /// Adds a new order line to the current order.
    /// </summary>
    /// <param name="orderLine"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void AddOrderLine(OrderLine orderLine)
    {
        if (orderLine == null)
        {
            throw new ArgumentNullException(nameof(orderLine));
        }
        if (_orderLines.Any(ol => ol.OrderItem.ItemId == orderLine.OrderItem.ItemId))
        {
            throw new InvalidOperationException($"An order line with Item ID {orderLine.OrderItem.ItemId} already exists in the order.");
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
    /// Updates the order details.
    /// </summary>
    /// <param name="orderPartner"></param>
    /// <param name="orderNumber"></param>
    /// <param name="orderDate"></param>
    /// <param name="orderTypeId"></param>
    /// <param name="orderStatusId"></param>
    /// <param name="additionalInformation"></param>
    /// <param name="orderLineInputs"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public void Update(
        OrderPartner orderPartner,
        string orderNumber,
        DateTime orderDate,
        long orderTypeId,
        long orderStatusId,
        string? additionalInformation,
        IEnumerable<OrderLineInput> orderLineInputs)
    {
        if (orderPartner == null!)
        {
            throw new ArgumentNullException(nameof(orderPartner));
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

        OrderPartner = orderPartner;
        OrderNumber = orderNumber;
        OrderDate = orderDate;
        OrderTypeId = orderTypeId;
        OrderStatusId = orderStatusId;
        AdditionalInformation = additionalInformation;

        SyncOrderLines(orderLineInputs);

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


    private void SyncOrderLines(IEnumerable<OrderLineInput> orderLineInputs)
    {
        var incomingIds = orderLineInputs.Select(i => i.Id).Where(Id => Id > 0).ToHashSet();
        _orderLines.RemoveAll(line => !incomingIds.Contains(line.Id));

        foreach (var input in orderLineInputs)
        {
            var existingLine = _orderLines.FirstOrDefault(l => l.Id == input.Id);
            if (existingLine != null)
            {
                existingLine.Update(input.OrderItem, input.OrderedQuantity, input.CancelledQuantity, input.FullfilledQuantity, input.Price, input.ExchangeRate, input.AdditionalInformation);
            }
            else
            {
                var newLine = OrderLine.Create(input.OrderItem, input.OrderedQuantity, input.Price, input.ExchangeRate, input.AdditionalInformation);
                _orderLines.Add(newLine);
            }
        }
    }

}
