using FluentAssertions;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.SharedKernel.ValueObjects;
using Ogma.Domain.UnitTests.Orders.Helpers;

namespace Ogma.Domain.UnitTests.Orders.Tests;

public class OrderLineTests
{
    [Fact]
    public void Create_ValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var orderedQuantity = 10m;
        var price = new Money(100m, "USD");
        var orderItem = OrdersTestData.CreateOrderItem();
        // Act
        var orderLine = OrderLine.Create(orderItem, orderedQuantity, price);
        // Assert
        orderLine.Should().NotBeNull();
        orderLine.OrderItem.Should().Be(orderItem);
        orderLine.OrderedQuantity.Should().Be(orderedQuantity);
        orderLine.Price.Should().Be(price);
        orderLine.CancelledQuantity.Should().Be(0);
        orderLine.FullfilledQuantity.Should().Be(0);
        orderLine.ExchangeRate.Should().BeNull();
        orderLine.AdditionalInformation.Should().BeEmpty();
    }

    [Fact]
    public void Create_ValidParametersWithExchangeRateAndAdditionalInfo_ShouldCreateInstance()
    {
        // Arrange
        var orderItem = OrdersTestData.CreateOrderItem();
        var orderedQuantity = 10m;
        var price = new Money(100m, "USD");
        var exchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var additionalInfo = "Special instructions";
        // Act
        var orderLine = OrderLine.Create(orderItem, orderedQuantity, price, exchangeRate, additionalInfo);
        // Assert
        orderLine.Should().NotBeNull();
        orderLine.OrderItem.Should().Be(orderItem);
        orderLine.OrderedQuantity.Should().Be(orderedQuantity);
        orderLine.Price.Should().Be(price);
        orderLine.ExchangeRate.Should().Be(exchangeRate);
        orderLine.AdditionalInformation.Should().Be(additionalInfo);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Create_InvalidOrderedQuantity_ShouldThrowArgumentException(decimal invalidQuantity)
    {
        // Arrange
        var orderItem = OrdersTestData.CreateOrderItem();
        var price = new Money(100m, "USD");
        // Act
        Action act = () => OrderLine.Create(orderItem, invalidQuantity, price);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_NullOrderItem_ShouldThrowArgumentNullException()
    {
        // Arrange
        var orderedQuantity = 10m;
        var price = new Money(100m, "USD");
        // Act
        Action act = () => OrderLine.Create(null!, orderedQuantity, price);
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Create_NullPrice_ShouldThrowArgumentNullException()
    {
        // Arrange
        var orderItem = OrdersTestData.CreateOrderItem();
        var orderedQuantity = 10m;
        Money price = null!;
        // Act
        Action act = () => OrderLine.Create(orderItem, orderedQuantity, price);
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Create_PriceWithDifferentCurrencyThanExchangeRate_ShouldThrowArgumentException()
    {
        // Arrange
        var orderItem = OrdersTestData.CreateOrderItem();
        var orderedQuantity = 10m;
        var price = new Money(100m, "USD");
        var exchangeRate = new ExchangeRate("EUR", "GBP", 0.75m);
        // Act
        Action act = () => OrderLine.Create(orderItem, orderedQuantity, price, exchangeRate, "Info");
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Reconstitute_ValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderItem = OrdersTestData.CreateOrderItem();
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 0m;
        var price = new Money(150m, "USD");
        // Act
        var orderLine = OrderLine.Reconstitute(id, orderItem, orderedQuantity, cancelledQuantity, fullfilledQuantity, price);
        // Assert
        orderLine.Should().NotBeNull();
        orderLine.Id.Should().Be(id);
        orderLine.OrderItem.Should().Be(orderItem);
        orderLine.OrderedQuantity.Should().Be(orderedQuantity);
        orderLine.Price.Should().Be(price);
        orderLine.CancelledQuantity.Should().Be(cancelledQuantity);
        orderLine.FullfilledQuantity.Should().Be(fullfilledQuantity);
        orderLine.ExchangeRate.Should().BeNull();
        orderLine.AdditionalInformation.Should().BeEmpty();
    }

    [Fact]
    public void Reconstitute_ValidParametersWithExchangeRateAndAdditionalInfo_ShouldCreateInstance()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderItem = OrdersTestData.CreateOrderItem();
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 0m;
        var price = new Money(150m, "USD");
        var exchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var additionalInfo = "Handle with care";
        // Act
        var orderLine = OrderLine.Reconstitute(id, orderItem, orderedQuantity, cancelledQuantity, fullfilledQuantity, price, exchangeRate, additionalInfo);
        // Assert
        orderLine.Should().NotBeNull();
        orderLine.Id.Should().Be(id);
        orderLine.OrderItem.Should().Be(orderItem);
        orderLine.OrderedQuantity.Should().Be(orderedQuantity);
        orderLine.CancelledQuantity.Should().Be(cancelledQuantity);
        orderLine.FullfilledQuantity.Should().Be(fullfilledQuantity);
        orderLine.Price.Should().Be(price);
        orderLine.ExchangeRate.Should().Be(exchangeRate);
        orderLine.AdditionalInformation.Should().Be(additionalInfo);
    }

    [Fact]
    public void Reconstitute_InvalidId_ShouldThrowArgumentException()
    {
        // Arrange
        var orderItem = OrdersTestData.CreateOrderItem();
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 0m;
        var price = new Money(150m, "USD");
        // Act
        Action act = () => OrderLine.Reconstitute(-1, orderItem, orderedQuantity, cancelledQuantity, fullfilledQuantity, price);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Reconstitute_NullOrderItem_ShouldThrowArgumentNullException()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 0m;
        var price = new Money(150m, "USD");
        // Act
        Action act = () => OrderLine.Reconstitute(id, null!, orderedQuantity, cancelledQuantity, fullfilledQuantity, price);
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void Reconstitute_InvalidOrderedQuantity_ShouldThrowArgumentException(decimal invalidQuantity)
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderItem = OrdersTestData.CreateOrderItem();
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 0m;
        var price = new Money(150m, "USD");
        // Act
        Action act = () => OrderLine.Reconstitute(id, orderItem, invalidQuantity, cancelledQuantity, fullfilledQuantity, price);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Reconstitute_NullPrice_ShouldThrowArgumentNullException()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderItem = OrdersTestData.CreateOrderItem();
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 0m;
        Money price = null!;
        // Act
        Action act = () => OrderLine.Reconstitute(id, orderItem, orderedQuantity, cancelledQuantity, fullfilledQuantity, price);
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Reconstitute_PriceWithDifferentCurrencyThanExchangeRate_ShouldThrowArgumentException()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderItem = OrdersTestData.CreateOrderItem();
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 0m;
        var price = new Money(150m, "USD");
        var exchangeRate = new ExchangeRate("EUR", "GBP", 0.75m);
        // Act
        Action act = () => OrderLine.Reconstitute(id, orderItem, orderedQuantity, cancelledQuantity, fullfilledQuantity, price, exchangeRate, "Info");
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_ValidParameters_ShouldUpdateProperties()
    {
        // Arrange
        var orderItem = OrdersTestData.CreateOrderItem();
        var orderLine = OrderLine.Create(orderItem, 10m, new Money(100m, "USD"));
        var newOrderItem = OrdersTestData.CreateOrderItem();
        var newOrderedQuantity = 20m;
        var newCancelledQuantity = 5m;
        var newFullfilledQuantity = 10m;
        var newPrice = new Money(150m, "USD");
        var newExchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var newAdditionalInfo = "Updated info";
        // Act
        orderLine.Update(newOrderItem, newOrderedQuantity, newCancelledQuantity, newFullfilledQuantity, newPrice, newExchangeRate, newAdditionalInfo);
        // Assert
        orderLine.OrderItem.Should().Be(newOrderItem);
        orderLine.OrderedQuantity.Should().Be(newOrderedQuantity);
        orderLine.CancelledQuantity.Should().Be(newCancelledQuantity);
        orderLine.FullfilledQuantity.Should().Be(newFullfilledQuantity);
        orderLine.Price.Should().Be(newPrice);
        orderLine.ExchangeRate.Should().Be(newExchangeRate);
        orderLine.AdditionalInformation.Should().Be(newAdditionalInfo);
    }

    [Fact]
    public void Update_NullOrderItem_ShouldThrowArgumentNullException()
    {
        // Arrange
        var orderItem = OrdersTestData.CreateOrderItem();
        var orderLine = OrderLine.Create(orderItem, 10m, new Money(100m, "USD"));
        var newOrderedQuantity = 20m;
        var newCancelledQuantity = 5m;
        var newFullfilledQuantity = 10m;
        var newPrice = new Money(150m, "USD");
        var newExchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var newAdditionalInfo = "Updated info";
        // Act
        Action act = () => orderLine.Update(null!, newOrderedQuantity, newCancelledQuantity, newFullfilledQuantity, newPrice, newExchangeRate, newAdditionalInfo);
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Update_InvalidOrderedQuantity_ShouldThrowArgumentException(decimal invalidQuantity)
    {
        // Arrange
        var orderItem = OrdersTestData.CreateOrderItem();
        var orderLine = OrderLine.Create(orderItem, 10m, new Money(100m, "USD"));
        var newOrderItem = OrdersTestData.CreateOrderItem();
        var newCancelledQuantity = 5m;
        var newFullfilledQuantity = 10m;
        var newPrice = new Money(150m, "USD");
        var newExchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var newAdditionalInfo = "Updated info";
        // Act
        Action act = () => orderLine.Update(newOrderItem, invalidQuantity, newCancelledQuantity, newFullfilledQuantity, newPrice, newExchangeRate, newAdditionalInfo);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_InvalidCancelledQuantity_ShouldThrowArgumentException()
    {
        // Arrange
        var orderItem = OrdersTestData.CreateOrderItem();
        var orderLine = OrderLine.Create(orderItem, 10m, new Money(100m, "USD"));
        var newOrderItem = OrdersTestData.CreateOrderItem();
        var newOrderedQuantity = 20m;
        var newCancelledQuantity = -1m;
        var newFullfilledQuantity = 10m;
        var newPrice = new Money(150m, "USD");
        var newExchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var newAdditionalInfo = "Updated info";
        // Act
        Action act = () => orderLine.Update(newOrderItem, newOrderedQuantity, newCancelledQuantity, newFullfilledQuantity, newPrice, newExchangeRate, newAdditionalInfo);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_InvalidFullfilledQuantity_ShouldThrowArgumentException()
    {
        // Arrange
        var orderItem = OrdersTestData.CreateOrderItem();
        var orderLine = OrderLine.Create(orderItem, 10m, new Money(100m, "USD"));
        var newOrderItem = OrdersTestData.CreateOrderItem();
        var newOrderedQuantity = 20m;
        var newCancelledQuantity = 5m;
        var newFullfilledQuantity = -2m;
        var newPrice = new Money(150m, "USD");
        var newExchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var newAdditionalInfo = "Updated info";
        // Act
        Action act = () => orderLine.Update(newOrderItem, newOrderedQuantity, newCancelledQuantity, newFullfilledQuantity, newPrice, newExchangeRate, newAdditionalInfo);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_FulfilledAndCancelledExceedOrdered_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var orderItem = OrdersTestData.CreateOrderItem();
        var orderLine = OrderLine.Create(orderItem, 10m, new Money(100m, "USD"));
        var newOrderItem = OrdersTestData.CreateOrderItem();
        var newOrderedQuantity = 20m;
        var newCancelledQuantity = 10m;
        var newFullfilledQuantity = 15m;
        var newPrice = new Money(150m, "USD");
        var newExchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var newAdditionalInfo = "Updated info";
        // Act
        Action act = () => orderLine.Update(newOrderItem, newOrderedQuantity, newCancelledQuantity, newFullfilledQuantity, newPrice, newExchangeRate, newAdditionalInfo);
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Update_NullPrice_ShouldThrowArgumentNullException()
    {
        // Arrange
        var orderLine = OrderLine.Create(OrdersTestData.CreateOrderItem(), 10m, new Money(100m, "USD"));
        var newOrderedQuantity = 20m;
        var newCancelledQuantity = 5m;
        var newFullfilledQuantity = 10m;
        Money newPrice = null!;
        var newExchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var newAdditionalInfo = "Updated info";
        // Act
        Action act = () => orderLine.Update(OrdersTestData.CreateOrderItem(), newOrderedQuantity, newCancelledQuantity, newFullfilledQuantity, newPrice, newExchangeRate, newAdditionalInfo);
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Update_PriceWithDifferentCurrencyThanExchangeRate_ShouldThrowArgumentException()
    {
        // Arrange
        var orderLine = OrderLine.Create(OrdersTestData.CreateOrderItem(), 10m, new Money(100m, "USD"));
        var newOrderedQuantity = 20m;
        var newCancelledQuantity = 5m;
        var newFullfilledQuantity = 10m;
        var newPrice = new Money(150m, "USD");
        var newExchangeRate = new ExchangeRate("EUR", "GBP", 0.75m);
        var newAdditionalInfo = "Updated info";
        // Act
        Action act = () => orderLine.Update(OrdersTestData.CreateOrderItem(), newOrderedQuantity, newCancelledQuantity, newFullfilledQuantity, newPrice, newExchangeRate, newAdditionalInfo);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ActiveQuantity_ShowsActiveQuantity()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 3m;
        var activeQuantityExpected = orderedQuantity - cancelledQuantity - fullfilledQuantity;
        var price = new Money(150m, "USD");
        var orderLine = OrderLine.Reconstitute(id, OrdersTestData.CreateOrderItem(), orderedQuantity, cancelledQuantity, fullfilledQuantity, price);
        // Act & Assert
        orderLine.ActiveQuantity.Should().Be(activeQuantityExpected);
    }

    [Fact]
    public void ConvertedPrice_ShouldConvertPriceUsingExchangeRate()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 3m;
        var price = new Money(150m, "USD");
        var exchangeRate = new ExchangeRate("USD", "EUR", 0.8m);
        var orderLine = OrderLine.Reconstitute(id, OrdersTestData.CreateOrderItem(), orderedQuantity, cancelledQuantity, fullfilledQuantity, price, exchangeRate, "Info");
        var expectedConvertedAmount = price.Amount * exchangeRate.Rate;
        // Act
        var convertedPrice = orderLine.ConvertedPrice;
        // Assert
        convertedPrice.Should().NotBeNull();
        convertedPrice!.Currency.Should().Be("EUR");
        convertedPrice.Amount.Should().Be(expectedConvertedAmount);
    }

    [Fact]
    public void ConvertedPrice_NoExchangeRate_ShouldBePrice()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 3m;
        var price = new Money(150m, "USD");
        var orderLine = OrderLine.Reconstitute(id, OrdersTestData.CreateOrderItem(), orderedQuantity, cancelledQuantity, fullfilledQuantity, price);
        // Act
        var convertedPrice = orderLine.ConvertedPrice;
        // Assert
        convertedPrice.Should().Be(price);
    }

    [Fact]
    public void LineActiveValue_ShowsActiveValue()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 3m;
        var price = new Money(150m, "USD");
        var orderLine = OrderLine.Reconstitute(id, OrdersTestData.CreateOrderItem(), orderedQuantity, cancelledQuantity, fullfilledQuantity, price);
        var expectedActiveValue = new Money(orderLine.ActiveQuantity * price.Amount, orderLine.ConvertedPrice.Currency);
        // Act
        var lineActiveValue = orderLine.LineActiveValue;
        // Assert
        lineActiveValue.Should().Be(expectedActiveValue);
    }

    [Fact]
    public void LineActiveConvertedValue_WithExchangeRate_ShowsActiveValueForConvertedPrice()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 3m;
        var price = new Money(150m, "USD");
        var exchangeRate = new ExchangeRate("USD", "EUR", 0.8m);
        var orderLine = OrderLine.Reconstitute(id, OrdersTestData.CreateOrderItem(), orderedQuantity, cancelledQuantity, fullfilledQuantity, price, exchangeRate, "Info");
        var expectedActiveValue = new Money(orderLine.ActiveQuantity * orderLine.ConvertedPrice!.Amount, orderLine.ConvertedPrice.Currency);
        // Act
        var lineActiveConvertedValue = orderLine.LineActiveConvertedValue;
        // Assert
        lineActiveConvertedValue.Should().Be(expectedActiveValue);
    }

    [Fact]
    public void LineActiveConvertedValue_NoExchangeRate_ShowsActiveValueForOriginalPrice()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 3m;
        var price = new Money(150m, "USD");
        var orderLine = OrderLine.Reconstitute(id, OrdersTestData.CreateOrderItem(), orderedQuantity, cancelledQuantity, fullfilledQuantity, price);
        var expectedActiveValue = new Money(orderLine.ActiveQuantity * price.Amount, price.Currency);
        // Act
        var lineActiveConvertedValue = orderLine.LineActiveConvertedValue;
        // Assert
        lineActiveConvertedValue.Should().Be(expectedActiveValue);
    }


}
