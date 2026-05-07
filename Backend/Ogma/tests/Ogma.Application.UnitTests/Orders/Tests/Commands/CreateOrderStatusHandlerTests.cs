using FluentAssertions;
using Moq;
using Ogma.Application.Orders.Commands;
using Ogma.Application.Orders.Extensions;
using Ogma.Application.UnitTests.Orders.Helpers;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.UnitTests.Orders.Tests.Commands;

public class CreateOrderStatusHandlerTests
{
    private readonly Mock<IOrderStatusRepository> _orderStatusRepositoryStub;
    private readonly CreateOrderStatusHandler _handler;

    public CreateOrderStatusHandlerTests()
    {
        _orderStatusRepositoryStub = new Mock<IOrderStatusRepository>();
        _handler = new CreateOrderStatusHandler(_orderStatusRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ValidOrderStatus_ReturnsCreatedOrderStatus()
    {
        // Arrange
        var command = new CreateOrderStatusCommand("Pending", "Pending");
        var createdOrderStatus = OrderStatus.Reconstitute(1L, command.Name, command.Description, OrdersTestData.GetMetadata());
        _orderStatusRepositoryStub.Setup(r => r.AddAsync(It.IsAny<OrderStatus>()))
            .ReturnsAsync(createdOrderStatus);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(createdOrderStatus.ToDto());
        _orderStatusRepositoryStub.Verify(r => r.AddAsync(It.Is<OrderStatus>(o =>
            o.Name == command.Name &&
            o.Description == command.Description)), Times.Once);
    }
}
