using FluentAssertions;
using Moq;
using Ogma.Application.Orders.Commands;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Ports;
using Ogma.Application.SharedKernel.Dtos;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Orders.Repositories;

namespace Ogma.Application.UnitTests.Orders.Tests.Commands;

public class CreateOrderHandlerTests
{
    private readonly CreateOrderHandler _handler;
    private readonly Mock<IOrderRepository> _orderRepository;
    private readonly Mock<IOrderTypeReader> _orderTypeReader;
    private readonly Mock<IOrderStatusReader> _orderStatusReader;
    private readonly Mock<ICatalogItemReader> _catalogItemReader;
    private readonly Mock<IPartnerReader> _partnerReader;

    public CreateOrderHandlerTests()
    {
        _orderRepository = new Mock<IOrderRepository>();
        _orderStatusReader = new Mock<IOrderStatusReader>();
        _orderTypeReader = new Mock<IOrderTypeReader>();
        _catalogItemReader = new Mock<ICatalogItemReader>();
        _partnerReader = new Mock<IPartnerReader>();
        _handler = new CreateOrderHandler(
            _orderRepository.Object,
            _orderTypeReader.Object,
            _orderStatusReader.Object,
            _catalogItemReader.Object,
            _partnerReader.Object);
    }

    [Fact]
    public async Task Handle_ValidOrder_ReturnsCreatedOrderDto()
    {
        // Arrange
        var orderPartner = new OrderPartnerDto(1L, "ACME Ltd");
        var itemsLookup = new Dictionary<long, OrderItemDto>
        {
            { 4L, new OrderItemDto(4L, "Item 1", "ITEM-1") },
            { 5L, new OrderItemDto(5L, "Item 2", "ITEM-2") },
        };
        var orderType = new OrderTypeDto(2L, "sales", "sales order");
        var orderStatus = new OrderStatusDto(3L, "pending", "pending order");
        var command = new CreateOrderCommand(
            orderPartner.PartnerId,
            "ORD-123",
            DateTime.UtcNow,
            orderType.Id,
            orderStatus.Id,
            "Super useful info",
            new List<CreateOrderLineDto>
            {
                new CreateOrderLineDto(
                    4L,
                    2m,
                    new MoneyDto(100m, "EUR"),
                    new ExchangeRateDto("EUR", "RON", 5),
                    "More information about the item 1"),
                new CreateOrderLineDto(
                    5L,
                    2m,
                    new MoneyDto(250m, "EUR"),
                    new ExchangeRateDto("EUR", "RON", 5),
                    "More information about the item 2"),
            });
        _partnerReader.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderPartner);
        _catalogItemReader.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(itemsLookup);
        _orderTypeReader.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderType);
        _orderStatusReader.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderStatus);
        // Using mirror return; focusing on property mapping rather than ID persistence.
        _orderRepository.Setup(r => r.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync((Order o) => o);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.OrderNumber.Should().Be(command.OrderNumber);
        result.OrderPartner.PartnerId.Should().Be(command.PartnerId);
        result.OrderDate.Should().Be(command.OrderDate);
        result.OrderType.Should().BeEquivalentTo(orderType);
        result.OrderStatus.Should().BeEquivalentTo(orderStatus);
        result.AdditionalInformation.Should().Be(command.AdditionalInformation);

        var cmdLines = command.OrderLines.ToArray();

        result.OrderLines.Should().SatisfyRespectively(
            firstLine =>
            {
                firstLine.OrderItem.ItemId.Should().Be(cmdLines[0].ItemId);
                firstLine.OrderedQuantity.Should().Be(cmdLines[0].OrderedQuantity);
                firstLine.Price.Should().BeEquivalentTo(cmdLines[0].Price);
                firstLine.ExchangeRate.Should().BeEquivalentTo(cmdLines[0].ExchangeRate);
                firstLine.AdditionalInformation.Should().Be(cmdLines[0].AdditionalInformation);
            },
            secondLine =>
            {
                secondLine.OrderItem.ItemId.Should().Be(cmdLines[1].ItemId);
                secondLine.OrderedQuantity.Should().Be(cmdLines[1].OrderedQuantity);
                secondLine.Price.Should().BeEquivalentTo(cmdLines[1].Price);
                secondLine.ExchangeRate.Should().BeEquivalentTo(cmdLines[1].ExchangeRate);
                secondLine.AdditionalInformation.Should().Be(cmdLines[1].AdditionalInformation);
            }
        );


    }

    [Fact]
    public async Task Handle_PartnerNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        _partnerReader.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((OrderPartnerDto?)null);
        var command = new CreateOrderCommand(
            999L, // Non-existent partner ID
            "ORD-123",
            DateTime.UtcNow,
            1L,
            1L,
            "Info",
            new List<CreateOrderLineDto>());
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.PartnerId.ToString()));
        _partnerReader.Verify(r => r.GetByIdAsync(command.PartnerId), Times.Once);
        _orderRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ItemNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var missingItemId = 99L;
        var orderPartner = new OrderPartnerDto(1L, "ACME Ltd");
        var itemsLookup = new Dictionary<long, OrderItemDto>
        {
            { 4L, new OrderItemDto(4L, "Item 1", "ITEM-1") }
        };
        var orderType = new OrderTypeDto(2L, "sales", "sales order");
        var orderStatus = new OrderStatusDto(3L, "pending", "pending order");
        var command = new CreateOrderCommand(
            orderPartner.PartnerId,
            "ORD-123",
            DateTime.UtcNow,
            orderType.Id,
            orderStatus.Id,
            "Super useful info",
            new List<CreateOrderLineDto>
            {
                new CreateOrderLineDto(
                    4L,
                    2m,
                    new MoneyDto(100m, "EUR"),
                    new ExchangeRateDto("EUR", "RON", 5),
                    "More information about the item 1"),
                new CreateOrderLineDto(
                    missingItemId,
                    2m,
                    new MoneyDto(250m, "EUR"),
                    new ExchangeRateDto("EUR", "RON", 5),
                    "More information about the item 2"),
            });
        var itemIds = command.OrderLines.Select(ol => ol.ItemId).Distinct();
        _partnerReader.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderPartner);
        _catalogItemReader.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(itemsLookup);
        _orderTypeReader.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderType);
        _orderStatusReader.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderStatus);
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(missingItemId.ToString()));
        _catalogItemReader.Verify(r => r.GetByIdsAsync(itemIds), Times.Once);
        _orderRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task Handle_OrderTypeNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var missingOrderTypeId = 99L;
        var orderPartner = new OrderPartnerDto(1L, "ACME Ltd");
        var itemsLookup = new Dictionary<long, OrderItemDto>
        {
            { 4L, new OrderItemDto(4L, "Item 1", "ITEM-1") },
            { 5L, new OrderItemDto(5L, "Item 2", "ITEM-2") },
        };
        var orderStatus = new OrderStatusDto(3L, "pending", "pending order");
        var command = new CreateOrderCommand(
            orderPartner.PartnerId,
            "ORD-123",
            DateTime.UtcNow,
            missingOrderTypeId,
            orderStatus.Id,
            "Super useful info",
            new List<CreateOrderLineDto>
            {
                new CreateOrderLineDto(
                    4L,
                    2m,
                    new MoneyDto(100m, "EUR"),
                    new ExchangeRateDto("EUR", "RON", 5),
                    "More information about the item 1"),
                new CreateOrderLineDto(
                    5L,
                    2m,
                    new MoneyDto(250m, "EUR"),
                    new ExchangeRateDto("EUR", "RON", 5),
                    "More information about the item 2"),
            });
        _partnerReader.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderPartner);
        _catalogItemReader.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(itemsLookup);
        _orderTypeReader.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((OrderTypeDto?)null);
        _orderStatusReader.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderStatus);
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(missingOrderTypeId.ToString()));
        _orderTypeReader.Verify(r => r.GetByIdAsync(missingOrderTypeId), Times.Once);
        _orderRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task Handle_OrderStatusNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var missingOrderStatusId = 99L;
        var orderPartner = new OrderPartnerDto(1L, "ACME Ltd");
        var itemsLookup = new Dictionary<long, OrderItemDto>
        {
            { 4L, new OrderItemDto(4L, "Item 1", "ITEM-1") },
            { 5L, new OrderItemDto(5L, "Item 2", "ITEM-2") },
        };
        var orderType = new OrderTypeDto(2L, "sales", "sales order");
        var command = new CreateOrderCommand(
            orderPartner.PartnerId,
            "ORD-123",
            DateTime.UtcNow,
            orderType.Id,
            missingOrderStatusId,
            "Super useful info",
            new List<CreateOrderLineDto>
            {
                new CreateOrderLineDto(
                    4L,
                    2m,
                    new MoneyDto(100m, "EUR"),
                    new ExchangeRateDto("EUR", "RON", 5),
                    "More information about the item 1"),
                new CreateOrderLineDto(
                    5L,
                    2m,
                    new MoneyDto(250m, "EUR"),
                    new ExchangeRateDto("EUR", "RON", 5),
                    "More information about the item 2"),
            });
        _partnerReader.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderPartner);
        _catalogItemReader.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<long>>()))
            .ReturnsAsync(itemsLookup);
        _orderTypeReader.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(orderType);
        _orderStatusReader.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((OrderStatusDto?)null);
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(missingOrderStatusId.ToString()));
        _orderStatusReader.Verify(r => r.GetByIdAsync(missingOrderStatusId), Times.Once);
        _orderRepository.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Never);
    }
}
