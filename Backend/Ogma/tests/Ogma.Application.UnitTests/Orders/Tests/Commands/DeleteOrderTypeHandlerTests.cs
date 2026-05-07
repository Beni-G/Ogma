using FluentAssertions;
using Moq;
using Ogma.Application.Orders.Commands;
using Ogma.Application.UnitTests.Orders.Helpers;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.UnitTests.Orders.Tests.Commands;

public class DeleteOrderTypeHandlerTests
{
    private readonly DeleteOrderTypeHandler _handler;
    private readonly Mock<IOrderTypeRepository> _orderTypeRepositoryStub;

    public DeleteOrderTypeHandlerTests()
    {
        _orderTypeRepositoryStub = new Mock<IOrderTypeRepository>();
        _handler = new DeleteOrderTypeHandler(_orderTypeRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ExistingOrderType_DeletesOrderType()
    {
        // Arrange
        var existingOrderType = OrderType.Reconstitute(1L, "sales", "sales", OrdersTestData.GetMetadata());
        var command = new DeleteOrderTypeCommand(existingOrderType.Id);
        _orderTypeRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(existingOrderType);
        // Act
        await _handler.Handle(command, CancellationToken.None);
        // Assert
        _orderTypeRepositoryStub.Verify(r => r.GetByIdAsync(existingOrderType.Id), Times.Once);
        _orderTypeRepositoryStub.Verify(r => r.DeleteAsync(existingOrderType), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new DeleteOrderTypeCommand(999L);
        _orderTypeRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((OrderType?)null);
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.Id.ToString()));
        _orderTypeRepositoryStub.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _orderTypeRepositoryStub.Verify(r => r.DeleteAsync(It.IsAny<OrderType>()), Times.Never);
    }
}
