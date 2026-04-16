using FluentAssertions;
using Moq;
using Ogma.Application.Orders.Commands;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Extensions;
using Ogma.Application.Orders.Ports;
using Ogma.Application.SharedKernel.Dtos;
using Ogma.Application.UnitTests.Orders.Helpers;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.UnitTests.Orders.Tests.Commands;

public class UpdateOrderHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryStub;
    private readonly Mock<IOrderReader> _orderReaderStub;
    private readonly Mock<IOrderTypeReader> _orderTypeReaderStub;
    private readonly Mock<IOrderStatusReader> _orderStatusReaderStub;
    private readonly Mock<ICatalogItemReader> _catalogItemReaderStub;
    private readonly Mock<IPartnerReader> _partnerReaderStub;
    private readonly UpdateOrderHandler _handler;

    public UpdateOrderHandlerTests()
    {
        _orderRepositoryStub = new Mock<IOrderRepository>();
        _orderReaderStub = new Mock<IOrderReader>();
        _orderTypeReaderStub = new Mock<IOrderTypeReader>();
        _orderStatusReaderStub = new Mock<IOrderStatusReader>();
        _catalogItemReaderStub = new Mock<ICatalogItemReader>();
        _partnerReaderStub = new Mock<IPartnerReader>();
        _handler = new UpdateOrderHandler(
            _orderRepositoryStub.Object,
            _orderReaderStub.Object,
            _orderTypeReaderStub.Object,
            _orderStatusReaderStub.Object,
            _catalogItemReaderStub.Object,
            _partnerReaderStub.Object);

        // Default setups for sad-paths
        _orderTypeReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new OrderTypeDto(1, "Sales", "Sales Order"));

        _orderStatusReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new OrderStatusDto(1, "Pending", "Pending"));

        _partnerReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new OrderPartnerDto(1, "Default Partner"));

        _orderRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new OrderBuilder().Build());

        _catalogItemReaderStub.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(new Dictionary<long, OrderItemDto>());
    }

    [Fact]
    public async Task Handle_ValidOrder_ReturnsUpdateOrder()
    {
        // Arrange
        var orderId = 1L;
        var orderType = new OrderTypeDto(1L, "sales", "sales order");
        var orderStatus = new OrderStatusDto(1L, "pending", "pending order");
        var existingOrder = new OrderBuilder()
            .WithId(orderId)
            .WithRandomLines()
            .Build();
        var command = new UpdateOrderCommand(orderId, existingOrder.OrderPartner.PartnerId, "ORD-2026-0001", DateTime.UtcNow, 1L, 1L, "Updated info", new List<UpdateOrderLineDto>
        {
            new UpdateOrderLineDto(0, 1L, 10, 0, 0, new MoneyDto(100, "USD"), null, "Line info"),
        });
        var itemsLookup = new Dictionary<long, OrderItemDto>
        {
            { 1L, new OrderItemDto(1L, "Item 1", "ITEM-1") }
        };
        var updatedOrder = new OrderBuilder()
            .FromCommand(command)
            .Build();
        _orderRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(existingOrder);
        _partnerReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new OrderPartnerDto(existingOrder.OrderPartner.PartnerId, existingOrder.OrderPartner.PartnerName));
        _orderTypeReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderType);
        _orderStatusReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderStatus);
        _catalogItemReaderStub.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(itemsLookup);
        _orderRepositoryStub.Setup(r => r.UpdateAsync(It.IsAny<Order>()))
            .ReturnsAsync(true);
        _orderReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(updatedOrder.ToDtoWithDto(orderType, orderStatus));
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        // Result
        result.Id.Should().Be(command.Id);
        result.OrderDate.Should().Be(command.OrderDate);
        result.OrderNumber.Should().Be(command.OrderNumber);
        result.OrderPartner.PartnerId.Should().Be(command.PartnerId);
        result.OrderTypeId.Should().Be(command.OrderTypeId);
        result.OrderStatusId.Should().Be(command.OrderStatusId);

        var firstResultLine = result.OrderLines.First();
        var firstCommandLine = command.OrderLines.First();

        firstResultLine.OrderItem.ItemId.Should().Be(firstCommandLine.ItemId);
        firstResultLine.OrderedQuantity.Should().Be(firstCommandLine.OrderedQuantity);

        // Calls
        _orderRepositoryStub.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
        _partnerReaderStub.Verify(r => r.GetByIdAsync(command.PartnerId), Times.Once);
        _orderTypeReaderStub.Verify(r => r.GetByIdAsync(command.OrderTypeId), Times.Once);
        _orderStatusReaderStub.Verify(r => r.GetByIdAsync(command.OrderStatusId), Times.Once);
        _catalogItemReaderStub.Verify(r => r.GetByIdsAsync(It.Is<IEnumerable<long>>(ids =>
            ids.SequenceEqual(command.OrderLines.Select(ol => ol.ItemId).Distinct()))), Times.Once);
        _orderRepositoryStub.Verify(r => r.UpdateAsync(It.Is<Order>(o =>
            o.Id == command.Id &&
            o.OrderDate == command.OrderDate &&
            o.OrderNumber == command.OrderNumber &&
            o.OrderTypeId == command.OrderTypeId &&
            o.OrderStatusId == command.OrderStatusId &&
            o.OrderLines.First().OrderItem.ItemId == command.OrderLines.First().ItemId)), Times.Once);
        _orderReaderStub.Verify(r => r.GetByIdAsync(command.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_OrderNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var orderId = 99L;
        var command = new UpdateOrderCommand(orderId, 1L, "ORD-2026-0001", DateTime.UtcNow, 11L, 111L, "Updated info", new List<UpdateOrderLineDto>
        {
            new UpdateOrderLineDto(0, 1L, 10, 0, 0, new MoneyDto(100, "USD"), null, "Line info"),
        });
        _orderRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Order?)null);
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(orderId.ToString()));
        _orderRepositoryStub.Verify(r => r.GetByIdAsync(orderId), Times.Once);
        _orderRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task Handle_PartnerNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var orderId = 1l;
        var command = new UpdateOrderCommand(orderId, 1L, "ORD-2026-0001", DateTime.UtcNow, 11L, 111L, "Updated info", new List<UpdateOrderLineDto>
        {
            new UpdateOrderLineDto(0, 1L, 10, 0, 0, new MoneyDto(100, "USD"), null, "Line info"),
        });
        _partnerReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((OrderPartnerDto?)null);
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.PartnerId.ToString()));
        _partnerReaderStub.Verify(r => r.GetByIdAsync(command.PartnerId), Times.Once);
        _orderRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task Handle_OrderTypeNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var orderId = 1l;
        var command = new UpdateOrderCommand(orderId, 1L, "ORD-2026-0001", DateTime.UtcNow, 11L, 111L, "Updated info", new List<UpdateOrderLineDto>
        {
            new UpdateOrderLineDto(0, 1L, 10, 0, 0, new MoneyDto(100, "USD"), null, "Line info"),
        });
        _orderTypeReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((OrderTypeDto?)null);
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.OrderTypeId.ToString()));
        _orderTypeReaderStub.Verify(r => r.GetByIdAsync(command.OrderTypeId), Times.Once);
        _orderRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task Handle_OrderStatusNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var orderId = 1l;
        var command = new UpdateOrderCommand(orderId, 1L, "ORD-2026-0001", DateTime.UtcNow, 11L, 111L, "Updated info", new List<UpdateOrderLineDto>
        {
            new UpdateOrderLineDto(0, 1L, 10, 0, 0, new MoneyDto(100, "USD"), null, "Line info"),
        });
        _orderStatusReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((OrderStatusDto?)null);
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.OrderStatusId.ToString()));
        _orderStatusReaderStub.Verify(r => r.GetByIdAsync(command.OrderStatusId), Times.Once);
        _orderRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ItemNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var orderId = 1L;
        var existingItemId = 1L;
        var missingItemId = 99L;
        var command = new UpdateOrderCommand(orderId, 1L, "ORD-2026-0001", DateTime.UtcNow, 11L, 111L, "Updated info", new List<UpdateOrderLineDto>
        {
            new UpdateOrderLineDto(0, existingItemId, 10, 0, 0, new MoneyDto(100, "USD"), null, "Line info 1"),
            new UpdateOrderLineDto(0, missingItemId, 20, 0, 0, new MoneyDto(200, "USD"), null, "Line info 2"),
        });
        _catalogItemReaderStub.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(new Dictionary<long, OrderItemDto> { { existingItemId, new OrderItemDto(existingItemId, "Name", "Code") } });
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(missingItemId.ToString()));
        _catalogItemReaderStub.Verify(r => r.GetByIdsAsync(It.Is<IEnumerable<long>>(ids =>
            ids.SequenceEqual(command.OrderLines.Select(ol => ol.ItemId).Distinct()))), Times.Once);
        _orderRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UpdateFails_ThrowsInvalidOperationException()
    {
        // Arrange
        var orderId = 1L;
        var orderType = new OrderTypeDto(1L, "sales", "sales order");
        var orderStatus = new OrderStatusDto(1L, "pending", "pending order");
        var existingOrder = new OrderBuilder()
            .WithId(orderId)
            .WithRandomLines()
            .Build();
        var command = new UpdateOrderCommand(orderId, existingOrder.OrderPartner.PartnerId, "ORD-2026-0001", DateTime.UtcNow, 11L, 111L, "Updated info", new List<UpdateOrderLineDto>
        {
            new UpdateOrderLineDto(0, 1L, 10, 0, 0, new MoneyDto(100, "USD"), null, "Line info"),
        });
        var itemsLookup = new Dictionary<long, OrderItemDto>
        {
            { 1L, new OrderItemDto(1L, "Item 1", "ITEM-1") }
        };
        var updatedOrder = new OrderBuilder()
            .FromCommand(command)
            .Build();
        _orderRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(existingOrder);
        _partnerReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new OrderPartnerDto(existingOrder.OrderPartner.PartnerId, existingOrder.OrderPartner.PartnerName));
        _orderTypeReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderType);
        _orderStatusReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderStatus);
        _catalogItemReaderStub.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(itemsLookup);
        _orderRepositoryStub.Setup(r => r.UpdateAsync(It.IsAny<Order>()))
            .ReturnsAsync(false);
        _orderReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(updatedOrder.ToDtoWithDto(orderType, orderStatus));
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex.Message.Contains(command.Id.ToString()));
    }

    [Fact]
    public async Task Handle_FailsUpdatedOrderRetrieval_ThrowsInvalidOperationException()
    {
        // Arrange
        var orderId = 1L;
        var orderType = new OrderTypeDto(1L, "sales", "sales order");
        var orderStatus = new OrderStatusDto(1L, "pending", "pending order");
        var existingOrder = new OrderBuilder()
            .WithId(orderId)
            .WithRandomLines()
            .Build();
        var command = new UpdateOrderCommand(orderId, existingOrder.OrderPartner.PartnerId, "ORD-2026-0001", DateTime.UtcNow, 11L, 111L, "Updated info", new List<UpdateOrderLineDto>
        {
            new UpdateOrderLineDto(0, 1L, 10, 0, 0, new MoneyDto(100, "USD"), null, "Line info"),
        });
        var itemsLookup = new Dictionary<long, OrderItemDto>
        {
            { 1L, new OrderItemDto(1L, "Item 1", "ITEM-1") }
        };
        var updatedOrder = new OrderBuilder()
            .FromCommand(command)
            .Build();
        _orderRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(existingOrder);
        _partnerReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new OrderPartnerDto(existingOrder.OrderPartner.PartnerId, existingOrder.OrderPartner.PartnerName));
        _orderTypeReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderType);
        _orderStatusReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderStatus);
        _catalogItemReaderStub.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(itemsLookup);
        _orderRepositoryStub.Setup(r => r.UpdateAsync(It.IsAny<Order>()))
            .ReturnsAsync(true);
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex.Message.Contains(command.Id.ToString()));
    }
}
