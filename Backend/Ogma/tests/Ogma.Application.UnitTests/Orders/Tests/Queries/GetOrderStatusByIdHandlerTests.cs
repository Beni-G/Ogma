using FluentAssertions;
using Moq;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;
using Ogma.Application.Orders.Queries;

namespace Ogma.Application.UnitTests.Orders.Tests.Queries;

public class GetOrderStatusByIdHandlerTests
{
    private readonly Mock<IOrderStatusReader> _orderStatusReaderStub;
    private readonly GetOrderStatusByIdHandler _handler;

    public GetOrderStatusByIdHandlerTests()
    {
        _orderStatusReaderStub = new Mock<IOrderStatusReader>();
        _handler = new GetOrderStatusByIdHandler(_orderStatusReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsOrderStatus()
    {
        // Arrange
        var orderStatus = new OrderStatusDto(1L, "Pending", "Pending");
        _orderStatusReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderStatus);
        // Act 
        var result = await _handler.Handle(new GetOrderStatusByIdQuery(orderStatus.Id), CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(orderStatus);
        _orderStatusReaderStub.Verify(r => r.GetByIdAsync(orderStatus.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var id = 999L;
        _orderStatusReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((OrderStatusDto?)null);
        // Act
        var act = () => _handler.Handle(new GetOrderStatusByIdQuery(id), CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _orderStatusReaderStub.Verify(r => r.GetByIdAsync(id), Times.Once);
    }
}
