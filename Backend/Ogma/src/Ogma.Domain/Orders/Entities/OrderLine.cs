using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Orders.Entities;

public class OrderLine : Entity<long>
{
    public long ItemId { get; private set; }
    public decimal OrderedQuantity { get; private set; }
    public decimal CancelledQuantity { get; private set; }
    public decimal FullfilledQuantity { get; private set; }
    public Money Price { get; private set; }
    public ExchangeRate? ExchangeRate { get; private set; }
    public string? AdditionalInformation { get; private set; }

    private OrderLine(long itemId, decimal orderedQuantity, Money price, ExchangeRate? exchangeRate = null, string? additionalInformation = "")
    {
        if (itemId <= 0)
        {
            throw new ArgumentException("Item ID must be a positive number.", nameof(itemId));
        }
        if (orderedQuantity <= 0)
        {
            throw new ArgumentException("Ordered quantity must be a positive number.", nameof(orderedQuantity));
        }
        if (price == null!)
        {
            throw new ArgumentNullException(nameof(price));
        }

        ItemId = itemId;
        OrderedQuantity = orderedQuantity;
        Price = price;
        ExchangeRate = exchangeRate;
        AdditionalInformation = additionalInformation;
    }

    private OrderLine(
        long id, 
        long itemId, 
        decimal orderedQuantity, 
        decimal cancelledQuantity, 
        decimal fullfilledQuantity, 
        Money price, 
        ExchangeRate? exchangeRate = null, 
        string? additionalInformation = "")
    {
        if (id <= 0)
        {
            throw new ArgumentException("ID must be a positive number.", nameof(id));
        }
        if (itemId <= 0)
        {
            throw new ArgumentException("Item ID must be a positive number.", nameof(itemId));
        }
        if (orderedQuantity <= 0)
        {
            throw new ArgumentException("Ordered quantity must be a positive number.", nameof(orderedQuantity));
        }
        if (cancelledQuantity < 0)
        {
            throw new ArgumentException("Cancelled quantity cannot be negative.", nameof(cancelledQuantity));
        }
        if (fullfilledQuantity < 0)
        {
            throw new ArgumentException("Fulfilled quantity cannot be negative.", nameof(fullfilledQuantity));
        }
        if (fullfilledQuantity + cancelledQuantity > orderedQuantity)
        {
            throw new InvalidOperationException("The sum of fulfilled and cancelled quantities cannot exceed the ordered quantity.");
        }
        if (price == null!)
        {
            throw new ArgumentNullException(nameof(price));
        }

        Id = id;
        ItemId = itemId;
        OrderedQuantity = orderedQuantity;
        CancelledQuantity = cancelledQuantity;
        FullfilledQuantity = fullfilledQuantity;
        Price = price;
        ExchangeRate = exchangeRate;
        AdditionalInformation = additionalInformation;
    }

    /// <summary>
    /// Creates a new instance of the OrderLine class with the specified item, quantity, price, and optional exchange
    /// rate and additional information.
    /// </summary>
    /// <param name="itemId">The unique identifier of the item to be included in the order line.</param>
    /// <param name="orderedQuantity">The quantity of the item to be ordered. Must be a non-negative value.</param>
    /// <param name="price">The unit price of the item, represented as a Money value.</param>
    /// <param name="exchangeRate">An optional exchange rate to apply to the price. If null, no currency conversion is performed.</param>
    /// <param name=""></param>
    /// <param name="additionalInformation">Optional additional information to associate with the order line. If not specified, an empty string is used.</param>
    /// <returns>A new OrderLine instance initialized with the provided values.</returns>
    public static OrderLine Create(long itemId, decimal orderedQuantity, Money price, ExchangeRate? exchangeRate = null, string? additionalInformation = "") 
        => new(itemId, orderedQuantity, price, exchangeRate, additionalInformation);

    /// <summary>
    /// Recreates an existing OrderLine instance from persisted data, restoring its state as recorded in storage.
    /// </summary>
    /// <remarks>This method is intended for reconstructing OrderLine entities from storage, such as when
    /// loading from a database or event store. It should not be used for creating new order lines in business logic, as
    /// it bypasses domain validation and invariants enforced by regular constructors or factory methods.</remarks>
    /// <param name="id">The unique identifier of the order line to reconstitute.</param>
    /// <param name="itemId">The unique identifier of the item associated with the order line.</param>
    /// <param name="orderedQuantity">The total quantity of the item that was originally ordered. Must be greater than or equal to zero.</param>
    /// <param name="cancelledQuantity">The quantity of the item that was cancelled. Must be zero or greater and should not exceed the ordered quantity.</param>
    /// <param name="fullfilledQuantity">The quantity of the item that has been fulfilled. Must be zero or greater and should not exceed the ordered
    /// quantity.</param>
    /// <param name="price">The price per unit of the item at the time of the order. Cannot be null.</param>
    /// <param name="exchangeRate">The exchange rate to apply if the price is in a different currency, or null if not applicable.</param>
    /// <param name="additionalInformation">Optional additional information or notes related to the order line. Can be null or empty.</param>
    /// <returns>An OrderLine instance with its state set to match the provided persisted values.</returns>
    public static OrderLine Reconstitute(
        long id, 
        long itemId, 
        decimal orderedQuantity, 
        decimal cancelledQuantity,
        decimal fullfilledQuantity,
        Money price, 
        ExchangeRate? exchangeRate = null, 
        string? additionalInformation = "") 
        => new(id, itemId, orderedQuantity, cancelledQuantity, fullfilledQuantity, price, exchangeRate, additionalInformation);

    /// <summary>
    /// Updates the item details with the specified quantities, price, exchange rate, and additional information.
    /// </summary>
    /// <param name="itemId">The unique identifier of the item to update. Must be a positive number.</param>
    /// <param name="orderedQuantity">The total quantity of the item that was ordered. Must be a positive value.</param>
    /// <param name="cancelledQuantity">The quantity of the item that was cancelled. Cannot be negative. The sum of cancelled and fulfilled quantities
    /// must not exceed the ordered quantity.</param>
    /// <param name="fullfilledQuantity">The quantity of the item that was fulfilled. Cannot be negative. The sum of fulfilled and cancelled quantities
    /// must not exceed the ordered quantity.</param>
    /// <param name="price">The price of the item. Cannot be null.</param>
    /// <param name="exchangeRate">The exchange rate to apply to the price, or null if no exchange rate is applicable.</param>
    /// <param name="additionalInformation">Additional information or notes related to the item update. Can be null or empty.</param>
    /// <exception cref="ArgumentException">Thrown if itemId is not positive, orderedQuantity is not positive, cancelledQuantity is negative, or
    /// fullfilledQuantity is negative.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the sum of fulfilled and cancelled quantities exceeds the ordered quantity.</exception>
    /// <exception cref="ArgumentNullException">Thrown if price is null.</exception>
    public void Update(
        long itemId,
        decimal orderedQuantity,
        decimal cancelledQuantity,
        decimal fullfilledQuantity,
        Money price,
        ExchangeRate? exchangeRate,
        string additionalInformation)
    {
        if (itemId <= 0)
        {
            throw new ArgumentException("Item ID must be a positive number.", nameof(itemId));
        }
        if (orderedQuantity <= 0)
        {
            throw new ArgumentException("Ordered quantity must be a positive number.", nameof(orderedQuantity));
        }
        if (cancelledQuantity < 0)
        {
            throw new ArgumentException("Cancelled quantity cannot be negative.", nameof(cancelledQuantity));
        }
        if (fullfilledQuantity < 0)
        {
            throw new ArgumentException("Fulfilled quantity cannot be negative.", nameof(fullfilledQuantity));
        }
        if (fullfilledQuantity + cancelledQuantity > orderedQuantity)
        {
            throw new InvalidOperationException("The sum of fulfilled and cancelled quantities cannot exceed the ordered quantity.");
        }
        if (price == null!)
        {
            throw new ArgumentNullException(nameof(price));
        }
        
        ItemId = itemId;
        OrderedQuantity = orderedQuantity;
        CancelledQuantity = cancelledQuantity;
        FullfilledQuantity = fullfilledQuantity;
        Price = price;
        ExchangeRate = exchangeRate;
        AdditionalInformation = additionalInformation;

        Touch();
    }

    /// <summary>
    /// Gets the quantity that remains active and unfulfilled for the order.
    /// </summary>
    public decimal ActiveQuantity => OrderedQuantity - CancelledQuantity - FullfilledQuantity;

    /// <summary>
    /// Gets the price converted to the target currency using the current exchange rate, if available.
    /// </summary>
    /// <remarks>If an exchange rate is not set, the original price is returned without conversion.</remarks>
    public Money ConvertedPrice => ExchangeRate! != null! ? ExchangeRate.Convert(Price) : Price;

    /// <summary>
    /// Gets the total monetary value for the active quantity of the line item.
    /// </summary>
    public Money LineActiveValue => new Money(Price.Amount * ActiveQuantity, Price.Currency);

    /// <summary>
    /// Gets the total value of the active line, converted to the target currency.
    /// </summary>
    public Money LineActiveConvertedValue => new Money(ConvertedPrice.Amount * ActiveQuantity, ConvertedPrice.Currency);
}
