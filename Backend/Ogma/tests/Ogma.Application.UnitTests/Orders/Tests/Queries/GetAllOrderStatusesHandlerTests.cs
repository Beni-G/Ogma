using FluentAssertions;
using Moq;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;
using Ogma.Application.Orders.Queries;

namespace Ogma.Application.UnitTests.Orders.Tests.Queries;

public class GetAllOrderStatusesHandlerTests
{
    private readonly Mock<IOrderStatusReader> _orderStatusReaderStub;
    private readonly GetAllOrderStatusesHandler _handler;

    public GetAllOrderStatusesHandlerTests()
    {
        _orderStatusReaderStub = new Mock<IOrderStatusReader>();
        _handler = new GetAllOrderStatusesHandler(_orderStatusReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllOrderStatuses()
    {
        // Arrange
        var orderStatuses = new List<OrderStatusDto>
        {
            new OrderStatusDto(1L, "Pending", "Pending"),
            new OrderStatusDto(2L, "Cancelled", "Cancelled")
        };
        _orderStatusReaderStub.Setup(r => r.GetAllAsync())
            .ReturnsAsync(orderStatuses);
        // Act
        var result = await _handler.Handle(new GetAllOrderStatusesQuery(), CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(orderStatuses);
        _orderStatusReaderStub.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NoPartnerStatuses_ReturnsEmptyList()
    {
        // Arrange
        var orderStatuses = new List<OrderStatusDto>();
        _orderStatusReaderStub.Setup(r => r.GetAllAsync())
            .ReturnsAsync(orderStatuses);
        // Act
        var result = await _handler.Handle(new GetAllOrderStatusesQuery(), CancellationToken.None);
        // Assert
        result.Should().BeEmpty();
        _orderStatusReaderStub.Verify(r => r.GetAllAsync(), Times.Once);
    }
}
