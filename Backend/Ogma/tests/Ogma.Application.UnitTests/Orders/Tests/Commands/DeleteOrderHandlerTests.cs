using FluentAssertions;
using Moq;
using Ogma.Application.Orders.Commands;
using Ogma.Application.UnitTests.Orders.Helpers;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.UnitTests.Orders.Tests.Commands;

public class DeleteOrderHandlerTests
{
    private readonly DeleteOrderHandler _handler;
    private readonly Mock<IOrderRepository> _orderRepositoryStub;

    public DeleteOrderHandlerTests()
    {
        _orderRepositoryStub = new Mock<IOrderRepository>();
        _handler = new DeleteOrderHandler(_orderRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ExistingOrder_DeletsOrder()
    {
        // Arrange
        var id = 1L;
        var order = new OrderBuilder()
            .WithId(id)
            .Build();
        _orderRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(order);
        // Act
        await _handler.Handle(new DeleteOrderCommand(id), CancellationToken.None);
        // Assert
        _orderRepositoryStub.Verify(r => r.GetByIdAsync(id), Times.Once);
        _orderRepositoryStub.Verify(r => r.DeleteAsync(order), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var id = 99L;
        _orderRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Order?)null);
        // Act
        var act = () => _handler.Handle(new DeleteOrderCommand(id), CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _orderRepositoryStub.Verify(r => r.GetByIdAsync(id), Times.Once);
        _orderRepositoryStub.Verify(r => r.DeleteAsync(It.IsAny<Order>()), Times.Never);

    }
}
