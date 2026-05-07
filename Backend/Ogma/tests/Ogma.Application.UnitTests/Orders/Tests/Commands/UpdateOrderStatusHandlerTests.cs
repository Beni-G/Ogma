using FluentAssertions;
using Moq;
using Ogma.Application.Orders.Commands;
using Ogma.Application.Orders.Extensions;
using Ogma.Application.UnitTests.Orders.Helpers;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.UnitTests.Orders.Tests.Commands;

public class UpdateOrderStatusHandlerTests
{
    private readonly Mock<IOrderStatusRepository> _OrderStatusRepositoryStub;
    private readonly UpdateOrderStatusHandler _handler;

    public UpdateOrderStatusHandlerTests()
    {
        _OrderStatusRepositoryStub = new Mock<IOrderStatusRepository>();
        _handler = new UpdateOrderStatusHandler(_OrderStatusRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ValidOrderStatus_ReturnsUpdatedOrderStatus()
    {
        // Arrange
        var existingOrderStatus = OrderStatus.Reconstitute(1L, "Valid", "Valid", OrdersTestData.GetMetadata());
        var command = new UpdateOrderStatusCommand(1L, "Approved", "Approved");
        var updatedOrderStatus = OrderStatus.Reconstitute(command.Id, command.Name, command.Description, OrdersTestData.GetMetadata());
        _OrderStatusRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(updatedOrderStatus);
        _OrderStatusRepositoryStub.Setup(r => r.UpdateAsync(updatedOrderStatus))
            .ReturnsAsync(true);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(updatedOrderStatus.ToDto());
        _OrderStatusRepositoryStub.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _OrderStatusRepositoryStub.Verify(r => r.UpdateAsync(It.Is<OrderStatus>(o =>
            o.Id == command.Id &&
            o.Name == command.Name &&
            o.Description == command.Description)), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new UpdateOrderStatusCommand(999L, "Approved", "Approved");
        _OrderStatusRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((OrderStatus?)null);
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Arrange
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.Id.ToString()));
        _OrderStatusRepositoryStub.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _OrderStatusRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<OrderStatus>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UpdateFails_ThrowsInvalidOperationException()
    {
        // Arrange
        var existingOrderStatus = OrderStatus.Reconstitute(1L, "Valid", "Valid", OrdersTestData.GetMetadata());
        var command = new UpdateOrderStatusCommand(1L, "Approved", "Approved");
        var updatedOrderStatus = OrderStatus.Reconstitute(command.Id, command.Name, command.Description, OrdersTestData.GetMetadata());
        _OrderStatusRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(updatedOrderStatus);
        _OrderStatusRepositoryStub.Setup(r => r.UpdateAsync(updatedOrderStatus))
            .ReturnsAsync(false);
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Arrange
        await act.Should().ThrowAsync<InvalidOperationException>();
        _OrderStatusRepositoryStub.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _OrderStatusRepositoryStub.Verify(r => r.UpdateAsync(It.Is<OrderStatus>(o =>
            o.Id == command.Id &&
            o.Name == command.Name &&
            o.Description == command.Description)), Times.Once);
    }
}
