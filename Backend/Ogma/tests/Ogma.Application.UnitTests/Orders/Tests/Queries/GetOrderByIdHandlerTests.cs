using FluentAssertions;
using Moq;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;
using Ogma.Application.Orders.Queries;
using Ogma.Application.UnitTests.Orders.Helpers;

namespace Ogma.Application.UnitTests.Orders.Tests.Queries;

public class GetOrderByIdHandlerTests
{
    private readonly GetOrderByIdHandler _handler;
    private readonly Mock<IOrderReader> _orderReaderStub;

    public GetOrderByIdHandlerTests()
    {
        _orderReaderStub = new Mock<IOrderReader>();
        _handler = new GetOrderByIdHandler(_orderReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsOrder()
    {
        // Arrange
        var order = new OrderBuilder()
            .WithRandomLines()
            .BuildDto();
        _orderReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(order);
        // Act
        var result = await _handler.Handle(new GetOrderByIdQuery(order.Id), CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(order);
        _orderReaderStub.Verify(r => r.GetByIdAsync(order.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var id = 999l;
        _orderReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((OrderDto?)null);
        // Act
        var act = () => _handler.Handle(new GetOrderByIdQuery(id), CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _orderReaderStub.Verify(r => r.GetByIdAsync(id), Times.Once);
    }
}
