using Ogma.Domain.Orders.ValueObjects;
using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Orders.Entities;

public class OrderLine : Entity<long>
{
    public OrderItem OrderItem { get; private set; }
    public decimal OrderedQuantity { get; private set; }
    public decimal CancelledQuantity { get; private set; }
    public decimal FullfilledQuantity { get; private set; }
    public Money Price { get; private set; }
    public ExchangeRate? ExchangeRate { get; private set; }
    public string? AdditionalInformation { get; private set; }

    private OrderLine(OrderItem orderItem, decimal orderedQuantity, Money price, ExchangeRate? exchangeRate = null, string? additionalInformation = "")
    {
        if (orderItem == null!)
        {
            throw new ArgumentNullException(nameof(orderItem));
        }
        if (orderedQuantity <= 0)
        {
            throw new ArgumentException("Ordered quantity must be a positive number.", nameof(orderedQuantity));
        }
        if (price == null!)
        {
            throw new ArgumentNullException(nameof(price));
        }
        if (price.Currency != exchangeRate?.BaseCurrency && exchangeRate! != null!)
        {
            throw new ArgumentException("Price currency must match the exchange rate's base currency.", nameof(price));
        }

        OrderItem = orderItem;
        OrderedQuantity = orderedQuantity;
        Price = price;
        ExchangeRate = exchangeRate;
        AdditionalInformation = additionalInformation;
    }

    private OrderLine(
        long id, 
        OrderItem orderItem, 
        decimal orderedQuantity, 
        decimal cancelledQuantity, 
        decimal fullfilledQuantity, 
        Money price, 
        ExchangeRate? exchangeRate = null, 
        string? additionalInformation = "")
    {
        if (id < 0)
        {
            throw new ArgumentException("ID must be a positive number.", nameof(id));
        }
        if (orderItem == null!)
        {
            throw new ArgumentNullException(nameof(orderItem));
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
        if (price.Currency != exchangeRate?.BaseCurrency && exchangeRate! != null!)
        {
            throw new ArgumentException("Price currency must match the exchange rate's base currency.", nameof(price));
        }

        Id = id;
        OrderItem = orderItem;
        OrderedQuantity = orderedQuantity;
        CancelledQuantity = cancelledQuantity;
        FullfilledQuantity = fullfilledQuantity;
        Price = price;
        ExchangeRate = exchangeRate;
        AdditionalInformation = additionalInformation;
    }


    /// <summary>
    /// Creates a new OrderLine instance with the specified item, quantity, price, exchange rate, and additional information.
    /// </summary>
    /// <param name="orderItem"></param>
    /// <param name="orderedQuantity"></param>
    /// <param name="price"></param>
    /// <param name="exchangeRate"></param>
    /// <param name="additionalInformation"></param>
    /// <returns></returns>
    public static OrderLine Create(OrderItem orderItem, decimal orderedQuantity, Money price, ExchangeRate? exchangeRate = null, string? additionalInformation = "") 
        => new(orderItem, orderedQuantity, price, exchangeRate, additionalInformation);

    /// <summary>
    /// Reconstitutes an OrderLine instance from existing data, including its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="orderItem"></param>
    /// <param name="orderedQuantity"></param>
    /// <param name="cancelledQuantity"></param>
    /// <param name="fullfilledQuantity"></param>
    /// <param name="price"></param>
    /// <param name="exchangeRate"></param>
    /// <param name="additionalInformation"></param>
    /// <returns></returns>
    public static OrderLine Reconstitute(
        long id, 
        OrderItem orderItem, 
        decimal orderedQuantity, 
        decimal cancelledQuantity,
        decimal fullfilledQuantity,
        Money price, 
        ExchangeRate? exchangeRate = null, 
        string? additionalInformation = "") 
        => new(id, orderItem, orderedQuantity, cancelledQuantity, fullfilledQuantity, price, exchangeRate, additionalInformation);

    /// <summary>
    /// Updates the order line with new values for item, quantities, price, exchange rate, and additional information.
    /// </summary>
    /// <param name="orderItem"></param>
    /// <param name="orderedQuantity"></param>
    /// <param name="cancelledQuantity"></param>
    /// <param name="fullfilledQuantity"></param>
    /// <param name="price"></param>
    /// <param name="exchangeRate"></param>
    /// <param name="additionalInformation"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void Update(
        OrderItem orderItem,
        decimal orderedQuantity,
        decimal cancelledQuantity,
        decimal fullfilledQuantity,
        Money price,
        ExchangeRate? exchangeRate,
        string additionalInformation)
    {
        if (orderItem == null!)
        {
            throw new ArgumentNullException(nameof(orderItem));
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
        if (price.Currency != exchangeRate?.BaseCurrency && exchangeRate! != null!)
        {
            throw new ArgumentException("Price currency must match the exchange rate's base currency.", nameof(price));
        }

        OrderItem = orderItem;
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
