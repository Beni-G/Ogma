using Bogus;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.ValueObjects;

namespace Ogma.Domain.UnitTests.Orders.Helpers;

public class OrderBuilder
{
    private static readonly Faker _faker = new();

    private long _id = _faker.Random.Long(1, 10000);
    private string _orderNumber = _faker.Random.Replace("ORD-2026-####");
    private DateTime _orderDate = _faker.Date.Recent(7);
    private long _orderTypeId = 1;
    private long _orderStatusId = 1;
    private string? _info = _faker.Lorem.Sentence();

    private OrderPartner _partner = new OrderPartner(
        _faker.Random.Long(1, 10000),
        _faker.Company.CompanyName()
    );

    private List<OrderLine> _orderLines = new();

    public OrderBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public OrderBuilder WithLine(OrderLine line)
    {
        _orderLines.Add(line);
        return this;
    }

    public OrderBuilder WithRandomLines(int count = 2)
    {
        for (int i = 0; i < count; i++)
        {
            _orderLines.Add(new OrderLineBuilder().Build());
        }
        return this;
    }

    public Order Build()
    {
        return Order.Reconstitute(
            _id,
            _partner,
            _orderNumber,
            _orderDate,
            _orderTypeId,
            _orderStatusId,
            OrdersTestData.GetMetadata(),
            _info,
            _orderLines.AsReadOnly()
        );
    }

}
