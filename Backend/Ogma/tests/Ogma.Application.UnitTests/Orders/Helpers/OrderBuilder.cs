using Bogus;
using Ogma.Application.Orders.Commands;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Extensions;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.ValueObjects;

namespace Ogma.Application.UnitTests.Orders.Helpers;

public class OrderBuilder
{
    private static readonly Faker _faker = new();

    private long _id = _faker.Random.Long(1, 10000);
    private string _orderNumber = _faker.Random.Replace("ORD-2026-####");
    private DateTime _orderDate = _faker.Date.Recent(7);
    private long _orderTypeId = 1;
    private long _orderStatusId = 1;
    private string? _info = _faker.Lorem.Sentence();

    private OrderTypeDto _orderTypeDto = new(1L, "sales", "Sales order");
    private OrderStatusDto _orderStatusDto = new(1, "approved", "Approved order");

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

    public OrderBuilder FromCommand(UpdateOrderCommand command)
    {
        _id = command.Id; // The ID is critical for Updates
        _partner = new OrderPartner(command.PartnerId, _faker.Company.CompanyName());
        _orderNumber = command.OrderNumber;
        _orderDate = command.OrderDate;
        _orderTypeId = command.OrderTypeId;
        _orderStatusId = command.OrderStatusId;
        _info = command.AdditionalInformation;

        _orderLines = command.OrderLines.Select(l => new OrderLineBuilder()
            .WithItemId(l.ItemId)
            .WithOrderedQuantity(l.OrderedQuantity)
            .WithCancelledQuantity(l.CancelledQuantity)
            .WithFulfilledQuantity(l.FullfilledQuantity)
            .WithPrice(l.Price.Amount, l.Price.Currency)
            .Build())
            .ToList();

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

    public OrderDto BuildDto()
    {
        var entity = Build();
        return entity.ToDtoWithDto(_orderTypeDto, _orderStatusDto);
    }
}
