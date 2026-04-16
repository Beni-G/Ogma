using FluentAssertions;
using Moq;
using Ogma.Application.Orders.Commands;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.UnitTests.Orders.Tests.Commands;

public class DeleteOrderStatusHandlerTests
{
    private readonly Mock<IOrderStatusRepository> _orderStatusRepositoryStub;
    private readonly DeleteOrderStatusHandler _handler;

    public DeleteOrderStatusHandlerTests()
    {
        _orderStatusRepositoryStub = new Mock<IOrderStatusRepository>();
        _handler = new DeleteOrderStatusHandler(_orderStatusRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ExistingOrderStatus_DeletesOrderStatus()
    {
        // Arrange
        var existingOrderStatus = OrderStatus.Reconstitute(1L, "Cancelled", "Cancelled");
        _orderStatusRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(existingOrderStatus);
        // Act
        await _handler.Handle(new DeleteOrderStatusCommand(existingOrderStatus.Id), CancellationToken.None);
        // Assert
        _orderStatusRepositoryStub.Verify(r => r.GetByIdAsync(existingOrderStatus.Id), Times.Once);
        _orderStatusRepositoryStub.Verify(r => r.DeleteAsync(existingOrderStatus), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new DeleteOrderStatusCommand(999L);
        _orderStatusRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((OrderStatus?)null);
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.Id.ToString()));
        _orderStatusRepositoryStub.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _orderStatusRepositoryStub.Verify(r => r.DeleteAsync(It.IsAny<OrderStatus>()), Times.Never);
    }
}
