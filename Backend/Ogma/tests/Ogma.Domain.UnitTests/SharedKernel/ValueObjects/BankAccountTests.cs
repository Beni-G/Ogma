using FluentAssertions;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.UnitTests.SharedKernel.ValueObjects;

public class BankAccountTests
{
    [Fact]
    public void Constructor_ValidValuesProvided_SetsPropertiesCorrectly()
    {
        // Arrange
        var bank = "Big Bank";
        var iban = "DE12 5001 0517 0648 4898 90";
        var currency = "eur";
        var bic = "COBADEFFXXX";
        // Act
        var bankAccount = new BankAccount(bank, iban, currency, bic);
        // Assert
        bankAccount.Bank.Should().Be(bank);
        bankAccount.Iban.Should().Be("DE12500105170648489890");
        bankAccount.Currency.Should().Be("EUR");
        bankAccount.Bic.Should().Be(bic);
    }

    [Fact]
    public void Constructor_NullOptionalParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var bank = "Big Bank";
        var iban = "DE12 5001 0517 0648 4898 90";
        var currency = "eur";
        // Act
        var bankAccount = new BankAccount(bank, iban, currency);
        // Assert
        bankAccount.Bank.Should().Be(bank);
        bankAccount.Iban.Should().Be("DE12500105170648489890");
        bankAccount.Currency.Should().Be("EUR");
        bankAccount.Bic.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidBank_ThrowsArgumentException(string invalidBank)
    {
        // Arrange
        var iban = "DE12 5001 0517 0648 4898 90";
        var currency = "eur";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new BankAccount(invalidBank!, iban, currency));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("DE12 5001 0517 0648 4898 91")]
    public void Constructor_InvalidIban_ThrowsArgumentException(string invalidIban)
    {
        // Arrange
        var bank = "Big Bank";
        var currency = "eur";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new BankAccount(bank, invalidIban, currency));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidCurrency_ThrowsArgumentException(string invalidCurrency)
    {
        // Arrange
        var bank = "Big Bank";
        var iban = "DE12 5001 0517 0648 4898 90";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new BankAccount(bank, iban, invalidCurrency));
    }

    [Fact]
    public void GetFormattedIban_ShouldReturnFormattedIban()
    {
        // Arrange
        var bankAccount = new BankAccount("Big Bank", "DE12500105170648489890", "EUR");
        // Act
        var formattedIban = bankAccount.GetFormattedIban();
        // Assert
        formattedIban.Should().Be("DE12 5001 0517 0648 4898 90");
    }
}
