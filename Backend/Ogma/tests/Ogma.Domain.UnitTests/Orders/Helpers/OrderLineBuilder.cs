using Bogus;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.ValueObjects;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.UnitTests.Orders.Helpers;

public class OrderLineBuilder
{
    private static readonly Faker _faker = new();

    private long _id = _faker.Random.Long(1, 1000);
    private long _itemId = _faker.Random.Long(1, 1000);
    private string _itemName = _faker.Commerce.ProductName();
    private string _itemSku = _faker.Commerce.Ean8();
    private decimal _ordered = 10;
    private decimal _cancelled = 0;
    private decimal _fulfilled = 0;
    private Money _price = new Money(100, "EUR");
    private ExchangeRate? _exchangeRate = null;
    private string _info = _faker.Lorem.Sentence();

    public OrderLineBuilder WithQuantities(decimal ordered, decimal cancelled = 0, decimal fulfilled = 0)
    {
        _ordered = ordered;
        _cancelled = cancelled;
        _fulfilled = fulfilled;
        return this;
    }

    public OrderLineBuilder WithItemId(long itemId)
    {
        _itemId = itemId;
        return this;
    }

    public OrderLineBuilder WithPrice(decimal amount, string currency)
    {
        _price = new Money(amount, currency);
        return this;
    }

    public OrderLineBuilder WithOrderedQuantity(decimal orderd)
    {
        _ordered = orderd;
        return this;
    }

    public OrderLineBuilder WithCancelledQuantity(decimal cancelled)
    {
        _cancelled = cancelled;
        return this;
    }

    public OrderLineBuilder WithFulfilledQuantity(decimal fulfilled)
    {
        _fulfilled = fulfilled;
        return this;
    }

    public OrderLine Build()
    {
        return OrderLine.Reconstitute(
            _id,
            new OrderItem(_itemId, _itemSku, _itemName),
            _ordered,
            _cancelled,
            _fulfilled,
            _price,
            OrdersTestData.GetMetadata(),
            _exchangeRate,
            _info
        );
    }
}
