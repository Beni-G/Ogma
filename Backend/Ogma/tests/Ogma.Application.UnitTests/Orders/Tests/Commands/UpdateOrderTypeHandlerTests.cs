using FluentAssertions;
using Moq;
using Ogma.Application.Orders.Commands;
using Ogma.Application.Orders.Extensions;
using Ogma.Application.UnitTests.Orders.Helpers;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.UnitTests.Orders.Tests.Commands;

public class UpdateOrderTypeHandlerTests
{
    private readonly Mock<IOrderTypeRepository> _orderTypeRepositoryStub;
    private readonly UpdateOrderTypeHandler _handler;

    public UpdateOrderTypeHandlerTests()
    {
        _orderTypeRepositoryStub = new Mock<IOrderTypeRepository>();
        _handler = new UpdateOrderTypeHandler(_orderTypeRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ValidOrderType_ReturnsUpdatedOrderTypeDto()
    {
        // Arrange
        var command = new UpdateOrderTypeCommand(1L, "sales", "sales");
        var existingOrderType = OrderType.Reconstitute(command.Id, "sale", "sale", OrdersTestData.GetMetadata());
        var updatedOrderType = OrderType.Reconstitute(command.Id, command.Code, command.Description, OrdersTestData.GetMetadata());
        _orderTypeRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(existingOrderType);
        _orderTypeRepositoryStub.Setup(r => r.UpdateAsync(It.IsAny<OrderType>()))
            .ReturnsAsync(true);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(updatedOrderType.ToDto());
        _orderTypeRepositoryStub.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _orderTypeRepositoryStub.Verify(r => r.UpdateAsync(It.Is<OrderType>(o =>
            o.Id == command.Id &&
            o.Code == command.Code &&
            o.Description == command.Description)), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new UpdateOrderTypeCommand(999L, "sales", "sales");
        _orderTypeRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((OrderType?)null);
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.Id.ToString()));
        _orderTypeRepositoryStub.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _orderTypeRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<OrderType>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UpdateFails_ThrowsInvalidOperationException()
    {
        // Arrange
        var command = new UpdateOrderTypeCommand(1L, "sales", "sales");
        var existingOrderType = OrderType.Reconstitute(command.Id, "sale", "sale", OrdersTestData.GetMetadata());
        var updatedOrderType = OrderType.Reconstitute(command.Id, command.Code, command.Description, OrdersTestData.GetMetadata());
        _orderTypeRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(existingOrderType);
        _orderTypeRepositoryStub.Setup(r => r.UpdateAsync(It.IsAny<OrderType>()))
            .ReturnsAsync(false);
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _orderTypeRepositoryStub.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _orderTypeRepositoryStub.Verify(r => r.UpdateAsync(It.Is<OrderType>(o =>
            o.Id == command.Id &&
            o.Code == command.Code &&
            o.Description == command.Description)), Times.Once);
    }
}
