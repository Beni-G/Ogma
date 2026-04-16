using FluentAssertions;
using Moq;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;
using Ogma.Application.Orders.Queries;

namespace Ogma.Application.UnitTests.Orders.Tests.Queries;

public class GetAllOrderTypesHandlerTests
{
    private readonly Mock<IOrderTypeReader> _orderTypeReaderStub;
    private readonly GetAllOrderTypesHandler _handler;

    public GetAllOrderTypesHandlerTests()
    {
        _orderTypeReaderStub = new Mock<IOrderTypeReader>();
        _handler = new GetAllOrderTypesHandler(_orderTypeReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllOrderTypes()
    {
        // Arrange
        var orderTypes = new List<OrderTypeDto>
        {
            new OrderTypeDto(1L, "sales", "sales order"),
            new OrderTypeDto(2l, "purchase", "purchase order")
        };
        _orderTypeReaderStub.Setup(r => r.GetAllAsync()).ReturnsAsync(orderTypes);
        // Act
        var result = await _handler.Handle(new GetAllOrderTypesQuery(), CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(orderTypes);
        _orderTypeReaderStub.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NoOrderTypes_ReturnsEmptyList()
    {
        // Arrange
        var orderTypes = new List<OrderTypeDto>();
        _orderTypeReaderStub.Setup(r => r.GetAllAsync()).ReturnsAsync(orderTypes);
        // Act
        var result = await _handler.Handle(new GetAllOrderTypesQuery(), CancellationToken.None);
        // Assert
        result.Should().BeEmpty();
        _orderTypeReaderStub.Verify(r => r.GetAllAsync(), Times.Once);
    }
}
