using FluentAssertions;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.UnitTests.Orders;

public class OrderTests
{
    [Fact]
    public void Create_ValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var partnerId = 1L;
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = 1L;
        var orderStatusId = 1L;
        // Act
        var order = Order.Create(partnerId, orderNumber, orderDate, orderTypeId, orderStatusId);
        // Assert
        order.Should().NotBeNull();
        order.PartnerId.Should().Be(partnerId);
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
        var partnerId = 1L;
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = 1L;
        var orderStatusId = 1L;
        var additionalInformation = "Test order";
        // Act
        var order = Order.Create(partnerId, orderNumber, orderDate, orderTypeId, orderStatusId, additionalInformation);
        // Assert
        order.Should().NotBeNull();
        order.PartnerId.Should().Be(partnerId);
        order.OrderNumber.Should().Be(orderNumber);
        order.OrderDate.Should().Be(orderDate);
        order.OrderTypeId.Should().Be(orderTypeId);
        order.OrderStatusId.Should().Be(orderStatusId);
        order.OrderLines.Should().BeEmpty();
        order.AdditionalInformation.Should().Be(additionalInformation);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_InvalidPartnerId_ShouldThrowArgumentException(long invalidPartnerId)
    {
        // Arrange
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = 1L;
        var orderStatusId = 1L;
        // Act
        Action act = () => Order.Create(invalidPartnerId, orderNumber, orderDate, orderTypeId, orderStatusId);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmptyOrderNumber_ShouldThrowArgumentException(string invalidOrderNumber)
    {
        // Arrange
        var partnerId = 1L;
        var orderDate = DateTime.Now;
        var orderTypeId = 1L;
        var orderStatusId = 1L;
        // Act
        Action act = () => Order.Create(partnerId, invalidOrderNumber, orderDate, orderTypeId, orderStatusId);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_InvalidOrderDate_ShouldThrowArgumentException()
    {
        // Arrange
        var partnerId = 1L;
        var orderNumber = "123";
        var invalidOrderDate = DateTime.MinValue;
        var orderTypeId = 1L;
        var orderStatusId = 1L;
        // Act
        Action act = () => Order.Create(partnerId, orderNumber, invalidOrderDate, orderTypeId, orderStatusId);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_InvalidOrderTypeId_ShouldThrowArgumentException(long invalidOrderTypeId)
    {
        // Arrange
        var partnerId = 1L;
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderStatusId = 1L;
        // Act
        Action act = () => Order.Create(partnerId, orderNumber, orderDate, invalidOrderTypeId, orderStatusId);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_InvalidOrderStatusId_ShouldThrowArgumentException(long invalidOrderStatusId)
    {
        // Arrange
        var partnerId = 1L;
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = 1L;
        // Act
        Action act = () => Order.Create(partnerId, orderNumber, orderDate, orderTypeId, invalidOrderStatusId);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Reconstitute_ValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var id = 1L;
        var partnerId = 1L;
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = 1L;
        var orderStatusId = 1L;
        // Act
        var order = Order.Reconstitute(id, partnerId, orderNumber, orderDate, orderTypeId, orderStatusId);
        // Assert
        order.Should().NotBeNull();
        order.Id.Should().Be(id);
        order.PartnerId.Should().Be(partnerId);
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
        var id = 1L;
        var partnerId = 1L;
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = 1L;
        var orderStatusId = 1L;
        var additionalInformation = "Test order";
        // Act
        var order = Order.Reconstitute(id, partnerId, orderNumber, orderDate, orderTypeId, orderStatusId, additionalInformation);
        // Assert
        order.Should().NotBeNull();
        order.Id.Should().Be(id);
        order.PartnerId.Should().Be(partnerId);
        order.OrderNumber.Should().Be(orderNumber);
        order.OrderDate.Should().Be(orderDate);
        order.OrderTypeId.Should().Be(orderTypeId);
        order.OrderStatusId.Should().Be(orderStatusId);
        order.OrderLines.Should().BeEmpty();
        order.AdditionalInformation.Should().Be(additionalInformation);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Reconstitute_InvalidId_ShouldThrowArgumentException(long invalidId)
    {
        // Arrange
        var partnerId = 1L;
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = 1L;
        var orderStatusId = 1L;
        // Act
        Action act = () => Order.Reconstitute(invalidId, partnerId, orderNumber, orderDate, orderTypeId, orderStatusId);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Reconstitute_InvalidPartnerId_ShouldThrowArgumentException(long invalidPartnerId)
    {
        // Arrange
        var id = 1L;
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = 1L;
        var orderStatusId = 1L;
        // Act
        Action act = () => Order.Reconstitute(id, invalidPartnerId, orderNumber, orderDate, orderTypeId, orderStatusId);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Reconstitute_NullOrEmptyOrderNumber_ShouldThrowArgumentException()
    {
        // Arrange
        var id = 1L;
        var partnerId = 1L;
        var invalidOrderNumber = "";
        var orderDate = DateTime.Now;
        var orderTypeId = 1L;
        var orderStatusId = 1L;
        // Act
        Action act = () => Order.Reconstitute(id, partnerId, invalidOrderNumber, orderDate, orderTypeId, orderStatusId);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Reconstitute_InvalidOrderDate_ShouldThrowArgumentException()
    {
        // Arrange
        var id = 1L;
        var partnerId = 1L;
        var orderNumber = "123";
        var invalidOrderDate = DateTime.MinValue;
        var orderTypeId = 1L;
        var orderStatusId = 1L;
        // Act
        Action act = () => Order.Reconstitute(id, partnerId, orderNumber, invalidOrderDate, orderTypeId, orderStatusId);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Reconstitute_InvalidOrderTypeId_ShouldThrowArgumentException(long invalidOrderTypeId)
    {
        // Arrange
        var id = 1L;
        var partnerId = 1L;
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderStatusId = 1L;
        // Act
        Action act = () => Order.Reconstitute(id, partnerId, orderNumber, orderDate, invalidOrderTypeId, orderStatusId);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Reconstitute_InvalidOrderStatusId_ShouldThrowArgumentException(long invalidOrderStatusId)
    {
        // Arrange
        var id = 1L;
        var partnerId = 1L;
        var orderNumber = "123";
        var orderDate = DateTime.Now;
        var orderTypeId = 1L;
        // Act
        Action act = () => Order.Reconstitute(id, partnerId, orderNumber, orderDate, orderTypeId, invalidOrderStatusId);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddOrderLine_ValidParameters_ShouldAddOrderLine()
    {
        // Arrange
        var order = Order.Create(1L, "123", DateTime.Now, 1L, 1L);
        var orderLine = OrderLine.Create(1L, 2, new Money(10m, "eur"));
        // Act
        order.AddOrderLine(orderLine);
        // Assert
        order.OrderLines.Should().HaveCount(1);
        var addedOrderLine = order.OrderLines.First();
        addedOrderLine.ItemId.Should().Be(orderLine.ItemId);
        addedOrderLine.OrderedQuantity.Should().Be(orderLine.OrderedQuantity);
        addedOrderLine.Price.Should().Be(orderLine.Price);
    }

    [Fact]
    public void AddOrderLine_NullOrderLine_ShouldThrowArgumentNullException()
    {
        // Arrange
        var order = Order.Create(1L, "123", DateTime.Now, 1L, 1L);
        // Act
        Action act = () => order.AddOrderLine(null!);
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddOrderLine_DuplicateOrderLine_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var order = Order.Create(1L, "123", DateTime.Now, 1L, 1L);
        var orderLine = OrderLine.Create(1L, 2, new Money(10m, "eur"));
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
        var order = Order.Create(1L, "123", DateTime.Now, 1L, 1L);
        var orderLine1 = OrderLine.Create(1L, 2, new Money(10m, "eur"));
        var orderLine2 = OrderLine.Create(2L, 3, new Money(15m, "usd"));
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
        var order = Order.Create(1L, "123", DateTime.Now, 1L, 1L);
        var orderLine1 = OrderLine.Create(1L, 2, new Money(10m, "eur"), new ExchangeRate("eur", "ron", 5m));
        var orderLine2 = OrderLine.Create(2L, 3, new Money(15m, "usd"), new ExchangeRate("usd", "gbp", 1.2m));
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
        var order = Order.Create(1L, "123", DateTime.Now, 1L, 1L);
        var orderLine = OrderLine.Reconstitute(1L, 1L, 2, 0, 0, new Money(10m, "eur"));
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
        var order = Order.Create(1L, "123", DateTime.Now, 1L, 1L);
        var orderLine = OrderLine.Reconstitute(1L, 1L, 2, 0, 0, new Money(10m, "eur"));
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
        var order = Order.Create(1L, "123", DateTime.Now, 1L, 1L);
        var orderLine1 = OrderLine.Reconstitute(1L, 1L, 2, 0, 0, new Money(10m, "eur"));
        var orderLine2 = OrderLine.Reconstitute(2L, 2L, 3, 0, 0, new Money(15m, "eur"));
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
        var order = Order.Create(1L, "123", DateTime.Now, 1L, 1L);
        var newPartnerId = 2L;
        var newOrderNumber = "456";
        var newOrderDate = DateTime.Now.AddDays(1);
        var newOrderTypeId = 2L;
        var newOrderStatusId = 2L;
        var newAdditionalInformation = "Updated order";
        // Act
        order.Update(newPartnerId, newOrderNumber, newOrderDate, newOrderTypeId, newOrderStatusId, newAdditionalInformation);
        // Assert
        order.PartnerId.Should().Be(newPartnerId);
        order.OrderNumber.Should().Be(newOrderNumber);
        order.OrderDate.Should().Be(newOrderDate);
        order.OrderTypeId.Should().Be(newOrderTypeId);
        order.OrderStatusId.Should().Be(newOrderStatusId);
        order.AdditionalInformation.Should().Be(newAdditionalInformation);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Update_InvalidPartnerId_ShouldThrowArgumentException(long invalidPartnerId)
    {
        // Arrange
        var order = Order.Create(1L, "123", DateTime.Now, 1L, 1L);
        var newOrderNumber = "456";
        var newOrderDate = DateTime.Now.AddDays(1);
        var newOrderTypeId = 2L;
        var newOrderStatusId = 2L;
        var additionalInformation = "Updated order";
        // Act
        Action act = () => order.Update(invalidPartnerId, newOrderNumber, newOrderDate, newOrderTypeId, newOrderStatusId, additionalInformation);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Update_InvalidOrderTypeId_ShouldThrowArgumentException(long invalidOrderTypeId)
    {
        // Arrange
        var order = Order.Create(1L, "123", DateTime.Now, 1L, 1L);
        var newPartnerId = 2L;
        var newOrderNumber = "456";
        var newOrderDate = DateTime.Now.AddDays(1);
        var newOrderStatusId = 2L;
        var additionalInformation = "Updated order";
        // Act
        Action act = () => order.Update(newPartnerId, newOrderNumber, newOrderDate, invalidOrderTypeId, newOrderStatusId, additionalInformation);
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
        var order = Order.Create(1L, "123", DateTime.Now, 1L, 1L);
        var newPartnerId = 2L;
        var newOrderDate = DateTime.Now.AddDays(1);
        var newOrderTypeId = 2L;
        var newOrderStatusId = 2L;
        var additionalInformation = "Updated order";
        // Act
        Action act = () => order.Update(newPartnerId, invalidOrderNumber, newOrderDate, newOrderTypeId, newOrderStatusId, additionalInformation);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_InvalidOrderDate_ShouldThrowArgumentException()
    {
        // Arrange
        var order = Order.Create(1L, "123", DateTime.Now, 1L, 1L);
        var newPartnerId = 2L;
        var newOrderNumber = "456";
        var invalidOrderDate = DateTime.MinValue;
        var newOrderTypeId = 2L;
        var newOrderStatusId = 2L;
        var additionalInformation = "Updated order";
        // Act
        Action act = () => order.Update(newPartnerId, newOrderNumber, invalidOrderDate, newOrderTypeId, newOrderStatusId, additionalInformation);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Update_InvalidOrderStatusId_ShouldThrowArgumentException(long invalidOrderStatusId)
    {
        // Arrange
        var order = Order.Create(1L, "123", DateTime.Now, 1L, 1L);
        var newPartnerId = 2L;
        var newOrderNumber = "456";
        var newOrderDate = DateTime.Now.AddDays(1);
        var newOrderTypeId = 2L;
        var additionalInformation = "Updated order";
        // Act
        Action act = () => order.Update(newPartnerId, newOrderNumber, newOrderDate, newOrderTypeId, invalidOrderStatusId, additionalInformation);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetTotalConvertedAmount_WithOrderLines_ShouldReturnTotalAmount()
    {
        // Arrange
        var order = Order.Create(1L, "123", DateTime.Now, 1L, 1L);
        var orderLine1 = OrderLine.Create(1L, 2, new Money(10m, "eur"), new ExchangeRate("eur", "ron", 5m));
        var orderLine2 = OrderLine.Create(2L, 3, new Money(15m, "eur"), new ExchangeRate("eur", "ron", 5m));
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
        var order = Order.Create(1L, "123", DateTime.Now, 1L, 1L);
        // Act
        var totalAmount = order.GetTotalConvertedAmount();
        // Assert
        totalAmount.Should().BeNull();
    }
}