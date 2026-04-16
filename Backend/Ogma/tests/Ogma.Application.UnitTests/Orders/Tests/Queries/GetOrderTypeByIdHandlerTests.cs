using FluentAssertions;
using Moq;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;
using Ogma.Application.Orders.Queries;

namespace Ogma.Application.UnitTests.Orders.Tests.Queries;

public class GetOrderTypeByIdHandlerTests
{
    private readonly Mock<IOrderTypeReader> _orderTypeReaderStub;
    private readonly GetOrderTypeByIdHandler _handler;

    public GetOrderTypeByIdHandlerTests()
    {
        _orderTypeReaderStub = new Mock<IOrderTypeReader>();
        _handler = new GetOrderTypeByIdHandler(_orderTypeReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsOrderType()
    {
        // Arrange
        var orderType = new OrderTypeDto(1L, "sales", "sales order");
        _orderTypeReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderType);
        // Act
        var result = await _handler.Handle(new GetOrderTypeByIdQuery(orderType.Id), CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(orderType);
        _orderTypeReaderStub.Verify(r => r.GetByIdAsync(orderType.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var id = 999L;
        _orderTypeReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((OrderTypeDto?)null);
        // Act
        var act = () => _handler.Handle(new GetOrderTypeByIdQuery(id), CancellationToken.None);
        /// Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _orderTypeReaderStub.Verify(r => r.GetByIdAsync(id), Times.Once);
    }
}
