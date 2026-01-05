using FluentAssertions;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.UnitTests.Orders;

public class OrderLineTests
{
    [Fact]
    public void Create_ValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var itemId = 1L;
        var orderedQuantity = 10m;
        var price = new Money(100m, "USD");
        // Act
        var orderLine = OrderLine.Create(itemId, orderedQuantity, price);
        // Assert
        orderLine.Should().NotBeNull();
        orderLine.ItemId.Should().Be(itemId);
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
        var itemId = 1L;
        var orderedQuantity = 10m;
        var price = new Money(100m, "USD");
        var exchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var additionalInfo = "Special instructions";
        // Act
        var orderLine = OrderLine.Create(itemId, orderedQuantity, price, exchangeRate, additionalInfo);
        // Assert
        orderLine.Should().NotBeNull();
        orderLine.ItemId.Should().Be(itemId);
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
        var itemId = 1L;
        var price = new Money(100m, "USD");
        // Act
        Action act = () => OrderLine.Create(itemId, invalidQuantity, price);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_InvalidItemId_ShouldThrowArgumentException(long invalidItemId)
    {
        // Arrange
        var orderedQuantity = 10m;
        var price = new Money(100m, "USD");
        // Act
        Action act = () => OrderLine.Create(invalidItemId, orderedQuantity, price);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_NullPrice_ShouldThrowArgumentNullException()
    {
        // Arrange
        var itemId = 1L;
        var orderedQuantity = 10m;
        Money price = null!;
        // Act
        Action act = () => OrderLine.Create(itemId, orderedQuantity, price);
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Reconstitute_ValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var id = 1L;
        var itemId = 2L;
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 0m;
        var price = new Money(150m, "USD");
        // Act
        var orderLine = OrderLine.Reconstitute(id, itemId, orderedQuantity, cancelledQuantity, fullfilledQuantity, price);
        // Assert
        orderLine.Should().NotBeNull();
        orderLine.Id.Should().Be(id);
        orderLine.ItemId.Should().Be(itemId);
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
        var id = 1L;
        var itemId = 2L;
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 0m;
        var price = new Money(150m, "USD");
        var exchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var additionalInfo = "Handle with care";
        // Act
        var orderLine = OrderLine.Reconstitute(id, itemId, orderedQuantity, cancelledQuantity, fullfilledQuantity, price, exchangeRate, additionalInfo);
        // Assert
        orderLine.Should().NotBeNull();
        orderLine.Id.Should().Be(id);
        orderLine.ItemId.Should().Be(itemId);
        orderLine.OrderedQuantity.Should().Be(orderedQuantity);
        orderLine.CancelledQuantity.Should().Be(cancelledQuantity);
        orderLine.FullfilledQuantity.Should().Be(fullfilledQuantity);
        orderLine.Price.Should().Be(price);
        orderLine.ExchangeRate.Should().Be(exchangeRate);
        orderLine.AdditionalInformation.Should().Be(additionalInfo);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Reconstitute_InvalidId_ShouldThrowArgumentException(long invalidId)
    {
        // Arrange
        var itemId = 2L;
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 0m;
        var price = new Money(150m, "USD");
        // Act
        Action act = () => OrderLine.Reconstitute(invalidId, itemId, orderedQuantity, cancelledQuantity, fullfilledQuantity, price);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Reconstitute_InvalidItemId_ShouldThrowArgumentException(long invalidItemId)
    {
        // Arrange
        var id = 1L;
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 0m;
        var price = new Money(150m, "USD");
        // Act
        Action act = () => OrderLine.Reconstitute(id, invalidItemId, orderedQuantity, cancelledQuantity, fullfilledQuantity, price);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void Reconstitute_InvalidOrderedQuantity_ShouldThrowArgumentException(decimal invalidQuantity)
    {
        // Arrange
        var id = 1L;
        var itemId = 2L;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 0m;
        var price = new Money(150m, "USD");
        // Act
        Action act = () => OrderLine.Reconstitute(id, itemId, invalidQuantity, cancelledQuantity, fullfilledQuantity, price);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Reconstitute_NullPrice_ShouldThrowArgumentNullException()
    {
        // Arrange
        var id = 1L;
        var itemId = 2L;
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 0m;
        Money price = null!;
        // Act
        Action act = () => OrderLine.Reconstitute(id, itemId, orderedQuantity, cancelledQuantity, fullfilledQuantity, price);
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Update_ValidParameters_ShouldUpdateProperties()
    {
        // Arrange
        var orderLine = OrderLine.Create(1L, 10m, new Money(100m, "USD"));
        var newItemId = 2L;
        var newOrderedQuantity = 20m;
        var newCancelledQuantity = 5m;
        var newFullfilledQuantity = 10m;
        var newPrice = new Money(150m, "USD");
        var newExchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var newAdditionalInfo = "Updated info";
        // Act
        orderLine.Update(newItemId, newOrderedQuantity, newCancelledQuantity, newFullfilledQuantity, newPrice, newExchangeRate, newAdditionalInfo);
        // Assert
        orderLine.ItemId.Should().Be(newItemId);
        orderLine.OrderedQuantity.Should().Be(newOrderedQuantity);
        orderLine.CancelledQuantity.Should().Be(newCancelledQuantity);
        orderLine.FullfilledQuantity.Should().Be(newFullfilledQuantity);
        orderLine.Price.Should().Be(newPrice);
        orderLine.ExchangeRate.Should().Be(newExchangeRate);
        orderLine.AdditionalInformation.Should().Be(newAdditionalInfo);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    public void Update_InvalidItemId_ShouldThrowArgumentException(long invalidItemId)
    {
        // Arrange
        var orderLine = OrderLine.Create(1L, 10m, new Money(100m, "USD"));
        var newOrderedQuantity = 20m;
        var newCancelledQuantity = 5m;
        var newFullfilledQuantity = 10m;
        var newPrice = new Money(150m, "USD");
        var newExchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var newAdditionalInfo = "Updated info";
        // Act
        Action act = () => orderLine.Update(invalidItemId, newOrderedQuantity, newCancelledQuantity, newFullfilledQuantity, newPrice, newExchangeRate, newAdditionalInfo);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Update_InvalidOrderedQuantity_ShouldThrowArgumentException(decimal invalidQuantity)
    {
        // Arrange
        var orderLine = OrderLine.Create(1L, 10m, new Money(100m, "USD"));
        var newItemId = 2L;
        var newCancelledQuantity = 5m;
        var newFullfilledQuantity = 10m;
        var newPrice = new Money(150m, "USD");
        var newExchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var newAdditionalInfo = "Updated info";
        // Act
        Action act = () => orderLine.Update(newItemId, invalidQuantity, newCancelledQuantity, newFullfilledQuantity, newPrice, newExchangeRate, newAdditionalInfo);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_InvalidCancelledQuantity_ShouldThrowArgumentException()
    {
        // Arrange
        var orderLine = OrderLine.Create(1L, 10m, new Money(100m, "USD"));
        var newItemId = 2L;
        var newOrderedQuantity = 20m;
        var newCancelledQuantity = -1m;
        var newFullfilledQuantity = 10m;
        var newPrice = new Money(150m, "USD");
        var newExchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var newAdditionalInfo = "Updated info";
        // Act
        Action act = () => orderLine.Update(newItemId, newOrderedQuantity, newCancelledQuantity, newFullfilledQuantity, newPrice, newExchangeRate, newAdditionalInfo);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_InvalidFullfilledQuantity_ShouldThrowArgumentException()
    {
        // Arrange
        var orderLine = OrderLine.Create(1L, 10m, new Money(100m, "USD"));
        var newItemId = 2L;
        var newOrderedQuantity = 20m;
        var newCancelledQuantity = 5m;
        var newFullfilledQuantity = -2m;
        var newPrice = new Money(150m, "USD");
        var newExchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var newAdditionalInfo = "Updated info";
        // Act
        Action act = () => orderLine.Update(newItemId, newOrderedQuantity, newCancelledQuantity, newFullfilledQuantity, newPrice, newExchangeRate, newAdditionalInfo);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_FulfilledAndCancelledExceedOrdered_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var orderLine = OrderLine.Create(1L, 10m, new Money(100m, "USD"));
        var newItemId = 2L;
        var newOrderedQuantity = 20m;
        var newCancelledQuantity = 10m;
        var newFullfilledQuantity = 15m;
        var newPrice = new Money(150m, "USD");
        var newExchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var newAdditionalInfo = "Updated info";
        // Act
        Action act = () => orderLine.Update(newItemId, newOrderedQuantity, newCancelledQuantity, newFullfilledQuantity, newPrice, newExchangeRate, newAdditionalInfo);
        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Update_NullPrice_ShouldThrowArgumentNullException()
    {
        // Arrange
        var orderLine = OrderLine.Create(1L, 10m, new Money(100m, "USD"));
        var newItemId = 2L;
        var newOrderedQuantity = 20m;
        var newCancelledQuantity = 5m;
        var newFullfilledQuantity = 10m;
        Money newPrice = null!;
        var newExchangeRate = new ExchangeRate("USD", "EUR", 0.85m);
        var newAdditionalInfo = "Updated info";
        // Act
        Action act = () => orderLine.Update(newItemId, newOrderedQuantity, newCancelledQuantity, newFullfilledQuantity, newPrice, newExchangeRate, newAdditionalInfo);
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ActiveQuantity_ShowsActiveQuantity()
    {
        // Arrange
        var id = 1L;
        var itemId = 2L;
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 3m;
        var activeQuantityExpected = orderedQuantity - cancelledQuantity - fullfilledQuantity;
        var price = new Money(150m, "USD");
        var orderLine = OrderLine.Reconstitute(id, itemId, orderedQuantity, cancelledQuantity, fullfilledQuantity, price);
        // Act & Assert
        orderLine.ActiveQuantity.Should().Be(activeQuantityExpected);
    }

    [Fact]
    public void ConvertedPrice_ShouldConvertPriceUsingExchangeRate()
    {
        // Arrange
        var id = 1L;
        var itemId = 2L;
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 3m;
        var price = new Money(150m, "USD");
        var exchangeRate = new ExchangeRate("USD", "EUR", 0.8m);
        var orderLine = OrderLine.Reconstitute(id, itemId, orderedQuantity, cancelledQuantity, fullfilledQuantity, price, exchangeRate, "Info");
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
        var id = 1L;
        var itemId = 2L;
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 3m;
        var price = new Money(150m, "USD");
        var orderLine = OrderLine.Reconstitute(id, itemId, orderedQuantity, cancelledQuantity, fullfilledQuantity, price);
        // Act
        var convertedPrice = orderLine.ConvertedPrice;
        // Assert
        convertedPrice.Should().Be(price);
    }

    [Fact]
    public void LineActiveValue_ShowsActiveValue()
    {
        // Arrange
        var id = 1L;
        var itemId = 2L;
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 3m;
        var price = new Money(150m, "USD");
        var orderLine = OrderLine.Reconstitute(id, itemId, orderedQuantity, cancelledQuantity, fullfilledQuantity, price);
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
        var id = 1L;
        var itemId = 2L;
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 3m;
        var price = new Money(150m, "USD");
        var exchangeRate = new ExchangeRate("USD", "EUR", 0.8m);
        var orderLine = OrderLine.Reconstitute(id, itemId, orderedQuantity, cancelledQuantity, fullfilledQuantity, price, exchangeRate, "Info");
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
        var id = 1L;
        var itemId = 2L;
        var orderedQuantity = 15m;
        var cancelledQuantity = 2m;
        var fullfilledQuantity = 3m;
        var price = new Money(150m, "USD");
        var orderLine = OrderLine.Reconstitute(id, itemId, orderedQuantity, cancelledQuantity, fullfilledQuantity, price);
        var expectedActiveValue = new Money(orderLine.ActiveQuantity * price.Amount, price.Currency);
        // Act
        var lineActiveConvertedValue = orderLine.LineActiveConvertedValue;
        // Assert
        lineActiveConvertedValue.Should().Be(expectedActiveValue);
    }

}
