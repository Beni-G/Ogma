using FluentAssertions;
using Moq;
using Ogma.Application.Orders.Commands;
using Ogma.Application.Orders.Extensions;
using Ogma.Application.UnitTests.Orders.Helpers;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.UnitTests.Orders.Tests.Commands;

public class CreateOrderTypeHandlerTests
{
    private readonly Mock<IOrderTypeRepository> _orderTypeRepositoryStub;
    private readonly CreateOrderTypeHandler _handler;

    public CreateOrderTypeHandlerTests()
    {
        _orderTypeRepositoryStub = new Mock<IOrderTypeRepository>();
        _handler = new CreateOrderTypeHandler(_orderTypeRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ValidOrderType_ReturnsCreatedOrderTypeDto()
    {
        // Arrange
        var command = new CreateOrderTypeCommand("sales", "sales");
        var createdOrderType = OrderType.Reconstitute(1L, command.Code, command.Description, OrdersTestData.GetMetadata());
        _orderTypeRepositoryStub.Setup(r => r.AddAsync(It.IsAny<OrderType>()))
            .ReturnsAsync(createdOrderType);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(createdOrderType.ToDto());
        _orderTypeRepositoryStub.Verify(r => r.AddAsync(It.Is<OrderType>(o =>
            o.Code == command.Code &&
            o.Description == command.Description)), Times.Once);
    }
}
