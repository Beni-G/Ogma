using FluentAssertions;
using Ogma.Domain.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogma.Domain.UnitTests.SharedKernel.ValueObjects;
public class MoneyTests
{
    [Fact]
    public void Constructor_ValidValuesProvided_SetsPropertiesCorrectly()
    {
        // Arrange
        var amount = 100m;
        var currency = "EUR";
        // Act
        var money = new Money(amount, currency);
        // Assert
        money.Amount.Should().Be(amount);
        money.Currency.Should().Be(currency);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidCurrency_ThrowsArgumentException(string invalidCurrency)
    {
        // Arrange
        var amount = 50m;
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Money(amount, invalidCurrency));
    }

    [Fact]
    public void Constructor_ShouldNormalizeCurrency_ToUpperCase()
    {
        // Arrange
        var amount = 75m;
        var currency = "usd";
        // Act
        var money = new Money(amount, currency);
        // Assert
        money.Currency.Should().Be("USD");
    }

    [Fact]
    public void Zero_ShouldReturnZeroAmountMoney()
    {
        // Arrange
        var currency = "GBP";
        // Act
        var zeroMoney = Money.Zero(currency);
        // Assert
        zeroMoney.Amount.Should().Be(0);
        zeroMoney.Currency.Should().Be(currency);
    }

    [Fact]
    public void Add_ValidValuesProvided_AddsAmount()
    {
        // Arrange
        var money1 = new Money(100m, "eur");
        var money2 = new Money(200m, "eur");
        // Act
        var result = money1.Add(money2);
        // Assert
        result.Amount.Should().Be(300m);
        result.Currency.Should().Be("EUR");
    }

    [Fact]
    public void Subtract_ValidValuesProvided_SubtractsAmount()
    {
        // Arrange
        var money1 = new Money(300m, "usd");
        var money2 = new Money(100m, "usd");
        // Act
        var result = money1.Subtract(money2);
        // Assert
        result.Amount.Should().Be(200m);
        result.Currency.Should().Be("USD");
    }

    [Fact]
    public void Multiply_ValidFactorProvided_MultipliesAmount()
    {
        // Arrange
        var money = new Money(150m, "usd");
        var factor = 2m;
        // Act
        var result = money.Multiply(factor);
        // Assert
        result.Amount.Should().Be(300m);
        result.Currency.Should().Be("USD");
    }

    [Fact]
    public void Divide_ValidDivisorProvided_DividesAmount()
    {
        // Arrange
        var money = new Money(200m, "eur");
        var divisor = 4m;
        // Act
        var result = money.Divide(divisor);
        // Assert
        result.Amount.Should().Be(50m);
        result.Currency.Should().Be("EUR");
    }

    [Fact]
    public void Divide_InvalidDivisor_ThrowsDivideByZeroException()
    {
        // Arrange
        var money = new Money(100m, "usd");
        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => money.Divide(0m));
    }

    [Fact]
    public void IsGreaterThan_ShouldReturnTrue_WhenFirstIsGreater()
    {
        // Arrange
        var money1 = new Money(200m, "usd");
        var money2 = new Money(100m, "usd");
        // Act
        var result = money1.IsGreaterThan(money2);
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsLessThan_ShouldReturnTrue_WhenFirstIsLesser()
    {
        // Arrange
        var money1 = new Money(50m, "usd");
        var money2 = new Money(100m, "usd");
        // Act
        var result = money1.IsLessThan(money2);
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equal_MoneyValues_ShouldBeEqual()
    {
        // Arrange & Act
        var a = new Money(100m, "USD");
        var b = new Money(100m, "USD");
        // Assert
        Assert.Equal(a, b);
        Assert.True(a.Equals(b));
        Assert.True(a == b); 
    }

    [Fact]
    public void DifferentAmount_ShouldNotBeEqual()
    {
        // Arrange & Act
        var a = new Money(100m, "USD");
        var b = new Money(200m, "USD");
        // Assert
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void DifferentCurrency_ShouldNotBeEqual()
    {
        // Arrange & Act
        var a = new Money(100m, "USD");
        var b = new Money(100m, "EUR");
        // Assert
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void EqualObjects_ShouldHaveSameHashCode()
    {
        // Arrange & Act
        var a = new Money(100m, "USD");
        var b = new Money(100m, "USD");
        // Assert
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Theory]
    [MemberData(nameof(InvalidOperations))]
    public void Operations_ShouldThrow_WhenCurrenciesDiffer(Action operation)
    {
        Assert.Throws<InvalidOperationException>(operation);
    }


    public static IEnumerable<object[]> InvalidOperations()
    {
        var usd = new Money(100m, "USD");
        var eur = new Money(100m, "EUR");

        yield return new object[] { (Action)(() => usd.Add(eur)) };
        yield return new object[] { (Action)(() => usd.Subtract(eur)) };
        yield return new object[] { (Action)(() => usd.IsGreaterThan(eur)) };
        yield return new object[] { (Action)(() => usd.IsLessThan(eur)) };
    }
}
