using FluentAssertions;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;
using Ogma.Domain.UnitTests.Orders.Helpers;

namespace Ogma.Domain.UnitTests.Orders.Tests;

public class OrderTests
{

    [Fact]
    public void Create_ValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var orderPartner = OrdersTestData.CreateOrderPartner();
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = OrdersTestData.NextId();
        var orderStatusId = OrdersTestData.NextId();
        // Act
        var order = Order.Create(orderPartner, orderNumber, orderDate, orderTypeId, orderStatusId);
        // Assert
        order.Should().NotBeNull();
        order.OrderPartner.Should().Be(orderPartner);
        order.OrderNumber.Should().Be(orderNumber);
        order.OrderDate.Should().Be(orderDate);
        order.OrderTypeId.Should().Be(orderTypeId);
        order.OrderStatusId.Should().Be(orderStatusId);
        order.OrderLines.Should().BeEmpty();
        order.AdditionalInformation.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithOptionalParameters_ShouldCreateInstance()
    {
        // Arrange
        var orderPartner = OrdersTestData.CreateOrderPartner();
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = OrdersTestData.NextId();
        var orderStatusId = OrdersTestData.NextId();
        var additionalInformation = "Test order";
        // Act
        var order = Order.Create(orderPartner, orderNumber, orderDate, orderTypeId, orderStatusId, additionalInformation);
        // Assert
        order.Should().NotBeNull();
        order.OrderPartner.Should().Be(orderPartner);
        order.OrderNumber.Should().Be(orderNumber);
        order.OrderDate.Should().Be(orderDate);
        order.OrderTypeId.Should().Be(orderTypeId);
        order.OrderStatusId.Should().Be(orderStatusId);
        order.OrderLines.Should().BeEmpty();
        order.AdditionalInformation.Should().Be(additionalInformation);
    }

    [Fact]
    public void Create_NullOrderPartner_ShouldThrowArgumentNullException()
    {
        // Arrange
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = OrdersTestData.NextId();
        var orderStatusId = OrdersTestData.NextId();
        // Act
        Action act = () => Order.Create(null!, orderNumber, orderDate, orderTypeId, orderStatusId);
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmptyOrderNumber_ShouldThrowArgumentException(string invalidOrderNumber)
    {
        // Arrange
        var orderPartner = OrdersTestData.CreateOrderPartner();
        var orderDate = DateTime.Now;
        var orderTypeId = OrdersTestData.NextId();
        var orderStatusId = OrdersTestData.NextId();
        // Act
        Action act = () => Order.Create(orderPartner, invalidOrderNumber, orderDate, orderTypeId, orderStatusId);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_InvalidOrderDate_ShouldThrowArgumentException()
    {
        // Arrange
        var orderPartner = OrdersTestData.CreateOrderPartner();
        var orderNumber = "123";
        var invalidOrderDate = DateTime.MinValue;
        var orderTypeId = OrdersTestData.NextId();
        var orderStatusId = OrdersTestData.NextId();
        // Act
        Action act = () => Order.Create(orderPartner, orderNumber, invalidOrderDate, orderTypeId, orderStatusId);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_InvalidOrderTypeId_ShouldThrowArgumentException(long invalidOrderTypeId)
    {
        // Arrange
        var orderPartner = OrdersTestData.CreateOrderPartner();
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderStatusId = OrdersTestData.NextId();
        // Act
        Action act = () => Order.Create(orderPartner, orderNumber, orderDate, invalidOrderTypeId, orderStatusId);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_InvalidOrderStatusId_ShouldThrowArgumentException(long invalidOrderStatusId)
    {
        // Arrange
        var orderPartner = OrdersTestData.CreateOrderPartner();
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = OrdersTestData.NextId();
        // Act
        Action act = () => Order.Create(orderPartner, orderNumber, orderDate, orderTypeId, invalidOrderStatusId);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Reconstitute_ValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderPartner = OrdersTestData.CreateOrderPartner();
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = OrdersTestData.NextId();
        var orderStatusId = OrdersTestData.NextId();
        // Act
        var order = Order.Reconstitute(id, orderPartner, orderNumber, orderDate, orderTypeId, orderStatusId, OrdersTestData.GetMetadata());
        // Assert
        order.Should().NotBeNull();
        order.Id.Should().Be(id);
        order.OrderPartner.Should().Be(orderPartner);
        order.OrderNumber.Should().Be(orderNumber);
        order.OrderDate.Should().Be(orderDate);
        order.OrderTypeId.Should().Be(orderTypeId);
        order.OrderStatusId.Should().Be(orderStatusId);
        order.OrderLines.Should().BeEmpty();
        order.AdditionalInformation.Should().BeEmpty();
    }

    [Fact]
    public void Reconstitute_WithOptionalParameters_ShouldCreateInstance()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderPartner = OrdersTestData.CreateOrderPartner();
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = OrdersTestData.NextId();
        var orderStatusId = OrdersTestData.NextId();
        var additionalInformation = "Test order";
        var orderLines = new List<OrderLine>()
        {
            OrderLine.Create(OrdersTestData.CreateOrderItem(), 10m, new Money(10m, "eur")),
            OrderLine.Create(OrdersTestData.CreateOrderItem(), 20m, new Money(5m, "eur"))
        };
        // Act
        var order = Order.Reconstitute(id, orderPartner, orderNumber, orderDate, orderTypeId, orderStatusId, OrdersTestData.GetMetadata(), additionalInformation, orderLines);
        // Assert
        order.Should().NotBeNull();
        order.Id.Should().Be(id);
        order.OrderPartner.Should().Be(orderPartner);
        order.OrderNumber.Should().Be(orderNumber);
        order.OrderDate.Should().Be(orderDate);
        order.OrderTypeId.Should().Be(orderTypeId);
        order.OrderStatusId.Should().Be(orderStatusId);
        order.OrderLines.Should().BeEquivalentTo(orderLines);
        order.AdditionalInformation.Should().Be(additionalInformation);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Reconstitute_InvalidId_ShouldThrowArgumentException(long invalidId)
    {
        // Arrange
        var orderPartner = OrdersTestData.CreateOrderPartner();
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = OrdersTestData.NextId();
        var orderStatusId = OrdersTestData.NextId();
        // Act
        Action act = () => Order.Reconstitute(invalidId, orderPartner, orderNumber, orderDate, orderTypeId, orderStatusId, OrdersTestData.GetMetadata());
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Reconstitute_NullOrderPartner_ShouldThrowArgumentNullException()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = OrdersTestData.NextId();
        var orderStatusId = OrdersTestData.NextId();
        // Act
        Action act = () => Order.Reconstitute(id, null!, orderNumber, orderDate, orderTypeId, orderStatusId, OrdersTestData.GetMetadata());
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Reconstitute_NullOrEmptyOrderNumber_ShouldThrowArgumentException()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderPartner = OrdersTestData.CreateOrderPartner();
        var invalidOrderNumber = "";
        var orderDate = DateTime.Now;
        var orderTypeId = OrdersTestData.NextId();
        var orderStatusId = OrdersTestData.NextId();
        // Act
        Action act = () => Order.Reconstitute(id, orderPartner, invalidOrderNumber, orderDate, orderTypeId, orderStatusId, OrdersTestData.GetMetadata());
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Reconstitute_InvalidOrderDate_ShouldThrowArgumentException()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderPartner = OrdersTestData.CreateOrderPartner();
        var orderNumber = "123";
        var invalidOrderDate = DateTime.MinValue;
        var orderTypeId = OrdersTestData.NextId();
        var orderStatusId = OrdersTestData.NextId();
        // Act
        Action act = () => Order.Reconstitute(id, orderPartner, orderNumber, invalidOrderDate, orderTypeId, orderStatusId, OrdersTestData.GetMetadata());
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Reconstitute_InvalidOrderTypeId_ShouldThrowArgumentException(long invalidOrderTypeId)
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderPartner = OrdersTestData.CreateOrderPartner();
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderStatusId = OrdersTestData.NextId();
        // Act
        Action act = () => Order.Reconstitute(id, orderPartner, orderNumber, orderDate, invalidOrderTypeId, orderStatusId, OrdersTestData.GetMetadata());
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Reconstitute_InvalidOrderStatusId_ShouldThrowArgumentException(long invalidOrderStatusId)
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderPartner = OrdersTestData.CreateOrderPartner();
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = OrdersTestData.NextId();
        // Act
        Action act = () => Order.Reconstitute(id, orderPartner, orderNumber, orderDate, orderTypeId, invalidOrderStatusId, OrdersTestData.GetMetadata());
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddOrderLine_ValidParameters_ShouldAddOrderLine()
    {
        // Arrange
        var order = Order.Create(OrdersTestData.CreateOrderPartner(), "123", DateTime.Now, 1L, 1L);
        var orderLine = OrderLine.Create(OrdersTestData.CreateOrderItem(), 2, new Money(10m, "eur"));
        // Act
        order.AddOrderLine(orderLine);
        // Assert
        order.OrderLines.Should().HaveCount(1);
        var addedOrderLine = order.OrderLines.First();
        addedOrderLine.OrderItem.ItemId.Should().Be(orderLine.OrderItem.ItemId);
        addedOrderLine.OrderedQuantity.Should().Be(orderLine.OrderedQuantity);
        addedOrderLine.Price.Should().Be(orderLine.Price);
    }

    [Fact]
    public void AddOrderLine_NullOrderLine_ShouldThrowArgumentNullException()
    {
        // Arrange
        var order = Order.Create(OrdersTestData.CreateOrderPartner(), "123", DateTime.Now, 1L, 1L);
        // Act
        Action act = () => order.AddOrderLine(null!);
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddOrderLine_DuplicateOrderLine_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var order = Order.Create(OrdersTestData.CreateOrderPartner(), "123", DateTime.Now, 1L, 1L);
        var orderLine = OrderLine.Create(OrdersTestData.CreateOrderItem(), 2m, new Money(10m, "eur"));
        order.AddOrderLine(orderLine);
        // Act
        Action act = () => order.AddOrderLine(orderLine);
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddOrderLine_WithDifferentCurrency_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var order = Order.Create(OrdersTestData.CreateOrderPartner(), "123", DateTime.Now, 1L, 1L);
        var orderLine1 = OrderLine.Create(OrdersTestData.CreateOrderItem(), 2, new Money(10m, "eur"));
        var orderLine2 = OrderLine.Create(OrdersTestData.CreateOrderItem(), 3, new Money(15m, "usd"));
        order.AddOrderLine(orderLine1);
        // Act
        Action act = () => order.AddOrderLine(orderLine2);
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddOrderLine_WithDifferenceConversionRate_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var order = Order.Create(OrdersTestData.CreateOrderPartner(), "123", DateTime.Now, 1L, 1L);
        var orderLine1 = OrderLine.Create(OrdersTestData.CreateOrderItem(), 2, new Money(10m, "eur"), new ExchangeRate("eur", "ron", 5m));
        var orderLine2 = OrderLine.Create(OrdersTestData.CreateOrderItem(), 3, new Money(15m, "usd"), new ExchangeRate("usd", "gbp", 1.2m));
        order.AddOrderLine(orderLine1);
        // Act
        Action act = () => order.AddOrderLine(orderLine2);
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void RemoveOrderLine_ExistingOrderLine_ShouldRemoveOrderLine()
    {
        // Arrange
        var order = Order.Create(OrdersTestData.CreateOrderPartner(), "123", DateTime.Now, 1L, 1L);
        var orderLine = OrderLine.Reconstitute(1L, OrdersTestData.CreateOrderItem(), 2, 0, 0, new Money(10m, "eur"), OrdersTestData.GetMetadata());
        order.AddOrderLine(orderLine);
        // Act
        order.RemoveOrderLine(1L);
        // Assert
        order.OrderLines.Should().BeEmpty();
    }

    [Fact]
    public void RemoveOrderLine_NonExistingOrderLine_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var order = Order.Create(OrdersTestData.CreateOrderPartner(), "123", DateTime.Now, 1L, 1L);
        var orderLine = OrderLine.Reconstitute(1L, OrdersTestData.CreateOrderItem(), 2, 0, 0, new Money(10m, "eur"), OrdersTestData.GetMetadata());
        order.AddOrderLine(orderLine);
        // Act
        Action act = () => order.RemoveOrderLine(2L);
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ClearOrderLines_ShouldRemoveAllOrderLines()
    {
        // Arrange
        var order = Order.Create(OrdersTestData.CreateOrderPartner(), "123", DateTime.Now, 1L, 1L);
        var orderLine1 = OrderLine.Reconstitute(1L, OrdersTestData.CreateOrderItem(), 2, 0, 0, new Money(10m, "eur"), OrdersTestData.GetMetadata());
        var orderLine2 = OrderLine.Reconstitute(2L, OrdersTestData.CreateOrderItem(), 3, 0, 0, new Money(15m, "eur"), OrdersTestData.GetMetadata());
        order.AddOrderLine(orderLine1);
        order.AddOrderLine(orderLine2);
        // Act
        order.ClearOrderLines();
        // Assert
        order.OrderLines.Should().BeEmpty();
    }

    [Fact]
    public void Update_ValidParameters_ShouldUpdateProperties()
    {
        // Arrange
        var orderLineToUpdate = new OrderLineBuilder()
            .Build();
        var orderLineToRemove = new OrderLineBuilder()
            .Build();
        var order = new OrderBuilder()
            .WithLine(orderLineToUpdate)
            .WithLine(orderLineToRemove)
            .Build();
        var newOrderPartner = OrdersTestData.CreateOrderPartner();
        var newOrderNumber = "456";
        var newOrderDate = DateTime.Now.AddDays(1);
        var newOrderTypeId = 2L;
        var newOrderStatusId = 2L;
        var newAdditionalInformation = "Updated order";
        var newOrderLines = new List<OrderLineInput>
        {
            new OrderLineInput(orderLineToUpdate.Id, OrdersTestData.CreateOrderItem(), 10, 0, 0, new Money(100, "EUR")),
            new OrderLineInput(0, OrdersTestData.CreateOrderItem(), 20, 0, 0, new Money(200, "EUR"))
        };
        // Act
        order.Update(newOrderPartner, newOrderNumber, newOrderDate, newOrderTypeId, newOrderStatusId, newAdditionalInformation, newOrderLines);
        // Assert
        order.OrderPartner.Should().Be(newOrderPartner);
        order.OrderNumber.Should().Be(newOrderNumber);
        order.OrderDate.Should().Be(newOrderDate);
        order.OrderTypeId.Should().Be(newOrderTypeId);
        order.OrderStatusId.Should().Be(newOrderStatusId);
        order.AdditionalInformation.Should().Be(newAdditionalInformation);
        order.OrderLines.Should().HaveCount(2);
        order.OrderLines.First().Id.Should().Be(newOrderLines.First().Id);
        order.OrderLines.First().OrderedQuantity.Should().Be(newOrderLines.First().OrderedQuantity);
        order.OrderLines.ElementAt(1).Id.Should().Be(newOrderLines.ElementAt(1).Id);
        order.OrderLines.ElementAt(1).OrderedQuantity.Should().Be(newOrderLines.ElementAt(1).OrderedQuantity);
        order.OrderLines.Should().NotContain(l => l.Id == orderLineToRemove.Id);
    }

    [Fact]
    public void Update_ValidParameters_ShouldUpdateMetadata()
    {
        // Arrange
        var orderLine = new OrderLineBuilder()
            .Build();
        var oldOrderLineMetadata = new EntityMetadata(orderLine.Metadata.CreatedAt, orderLine.Metadata.UpdatedAt, orderLine.Metadata.Version);
        var order = new OrderBuilder()
            .WithLine(orderLine)
            .Build();
        var oldOrderMetadata = new EntityMetadata(order.Metadata.CreatedAt, order.Metadata.UpdatedAt, order.Metadata.Version);
        var newOrderLines = new List<OrderLineInput>
        {
            new OrderLineInput(orderLine.Id, OrdersTestData.CreateOrderItem(), 10, 0, 0, new Money(100, "EUR"))
        };
        // Act
        order.Update(OrdersTestData.CreateOrderPartner(), "123", DateTime.UtcNow.AddDays(1), 2L, 2L, "info", newOrderLines);
        // Assert
        order.Metadata.CreatedAt.Should().Be(oldOrderMetadata.CreatedAt);
        order.Metadata.UpdatedAt.Should().BeAfter(oldOrderMetadata.UpdatedAt);
        order.Metadata.Version.Should().Be(oldOrderMetadata.Version + 1);
        order.OrderLines.First().Metadata.CreatedAt.Should().Be(oldOrderLineMetadata.CreatedAt);
        order.OrderLines.First().Metadata.UpdatedAt.Should().BeAfter(oldOrderLineMetadata.UpdatedAt);
        order.OrderLines.First().Metadata.Version.Should().Be(oldOrderLineMetadata.Version + 1);
    }

    [Fact]
    public void Update_NullOrderPartner_ShouldThrowArgumentNullException()
    {
        // Arrange
        var order = Order.Create(OrdersTestData.CreateOrderPartner(), "123", DateTime.Now, 1L, 1L);
        var newOrderNumber = "456";
        var newOrderDate = DateTime.Now.AddDays(1);
        var newOrderTypeId = 2L;
        var newOrderStatusId = 2L;
        var additionalInformation = "Updated order";
        // Act
        Action act = () => order.Update(null!, newOrderNumber, newOrderDate, newOrderTypeId, newOrderStatusId, additionalInformation, new List<OrderLineInput>());
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Update_InvalidOrderTypeId_ShouldThrowArgumentException(long invalidOrderTypeId)
    {
        // Arrange
        var order = Order.Create(OrdersTestData.CreateOrderPartner(), "123", DateTime.Now, 1L, 1L);
        var newOrderPartner = OrdersTestData.CreateOrderPartner();
        var newOrderNumber = "456";
        var newOrderDate = DateTime.Now.AddDays(1);
        var newOrderStatusId = 2L;
        var additionalInformation = "Updated order";
        // Act
        Action act = () => order.Update(newOrderPartner, newOrderNumber, newOrderDate, invalidOrderTypeId, newOrderStatusId, additionalInformation, new List<OrderLineInput>());
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_NullOrEmptyOrderNumber_ShouldThrowArgumentException(string invalidOrderNumber)
    {
        // Arrange
        var order = Order.Create(OrdersTestData.CreateOrderPartner(), "123", DateTime.Now, 1L, 1L);
        var newOrderPartner = OrdersTestData.CreateOrderPartner();
        var newOrderDate = DateTime.Now.AddDays(1);
        var newOrderTypeId = 2L;
        var newOrderStatusId = 2L;
        var additionalInformation = "Updated order";
        // Act
        Action act = () => order.Update(newOrderPartner, invalidOrderNumber, newOrderDate, newOrderTypeId, newOrderStatusId, additionalInformation, new List<OrderLineInput>());
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_InvalidOrderDate_ShouldThrowArgumentException()
    {
        // Arrange
        var order = Order.Create(OrdersTestData.CreateOrderPartner(), "123", DateTime.Now, 1L, 1L);
        var newOrderPartner = OrdersTestData.CreateOrderPartner();
        var newOrderNumber = "456";
        var invalidOrderDate = DateTime.MinValue;
        var newOrderTypeId = 2L;
        var newOrderStatusId = 2L;
        var additionalInformation = "Updated order";
        // Act
        Action act = () => order.Update(newOrderPartner, newOrderNumber, invalidOrderDate, newOrderTypeId, newOrderStatusId, additionalInformation, new List<OrderLineInput>());
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Update_InvalidOrderStatusId_ShouldThrowArgumentException(long invalidOrderStatusId)
    {
        // Arrange
        var order = Order.Create(OrdersTestData.CreateOrderPartner(), "123", DateTime.Now, 1L, 1L);
        var newOrderPartner = OrdersTestData.CreateOrderPartner();
        var newOrderNumber = "456";
        var newOrderDate = DateTime.Now.AddDays(1);
        var newOrderTypeId = 2L;
        var additionalInformation = "Updated order";
        // Act
        Action act = () => order.Update(newOrderPartner, newOrderNumber, newOrderDate, newOrderTypeId, invalidOrderStatusId, additionalInformation, new List<OrderLineInput>());
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetTotalConvertedAmount_WithOrderLines_ShouldReturnTotalAmount()
    {
        // Arrange
        var order = Order.Create(OrdersTestData.CreateOrderPartner(), "123", DateTime.Now, 1L, 1L);
        var orderLine1 = OrderLine.Create(OrdersTestData.CreateOrderItem(), 2, new Money(10m, "eur"), new ExchangeRate("eur", "ron", 5m));
        var orderLine2 = OrderLine.Create(OrdersTestData.CreateOrderItem(), 3, new Money(15m, "eur"), new ExchangeRate("eur", "ron", 5m));
        order.AddOrderLine(orderLine1);
        order.AddOrderLine(orderLine2);
        // Act
        var totalAmount = order.GetTotalConvertedAmount();
        // Assert
        totalAmount.Should().NotBeNull();
        totalAmount!.Amount.Should().Be((10m * 2 + 15m * 3) * 5m);
        totalAmount.Currency.Should().Be("RON");
    }

    [Fact]
    public void GetTotalConvertedAmount_NoOrderLines_ShouldReturnNull()
    {
        // Arrange
        var order = Order.Create(OrdersTestData.CreateOrderPartner(), "123", DateTime.Now, 1L, 1L);
        // Act
        var totalAmount = order.GetTotalConvertedAmount();
        // Assert
        totalAmount.Should().BeNull();
    }
}