using FluentAssertions;
using Moq;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;
using Ogma.Application.Orders.Queries;
using Ogma.Application.UnitTests.Orders.Helpers;

namespace Ogma.Application.UnitTests.Orders.Tests.Queries;

public class GetAllOrdersHandlerTests
{
    private readonly GetAllOrdersHandler _handler;
    private readonly Mock<IOrderReader> _orderReaderStub;

    public GetAllOrdersHandlerTests()
    {
        _orderReaderStub = new Mock<IOrderReader>();
        _handler = new GetAllOrdersHandler(_orderReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllOrders()
    {
        // Arrange
        var orders = new List<OrderDto>
        {
            new OrderBuilder().BuildDto(),
            new OrderBuilder().BuildDto()
        };
        _orderReaderStub.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);
        // Act
        var result = await _handler.Handle(new GetAllOrdersQuery(), CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(orders);
        _orderReaderStub.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NoOrders_ReturnsEmptyList()
    {
        // Arrange
        var orders = new List<OrderDto>();
        _orderReaderStub.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);
        // Act
        var result = await _handler.Handle(new GetAllOrdersQuery(), CancellationToken.None);
        // Assert
        result.Should().BeEmpty();
        _orderReaderStub.Verify(r => r.GetAllAsync(), Times.Once);
    }
}
