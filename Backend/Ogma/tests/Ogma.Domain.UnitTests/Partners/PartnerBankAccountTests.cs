using FluentAssertions;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.UnitTests.Partners;

public class PartnerBankAccountTests
{
    [Fact]
    public void Create_ValidParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var bankAccount = new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX");
        var isDefault = true;
        // Act
        var partnerBankAccount = PartnerBankAccount.Create(bankAccount, isDefault);
        // Assert
        partnerBankAccount.BankAccount.Should().Be(bankAccount);
        partnerBankAccount.IsDefault.Should().BeTrue();
    }

    [Fact]
    public void Create__OptionalParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var bankAccount = new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX");
        // Act
        var partnerBankAccount = PartnerBankAccount.Create(bankAccount);
        // Assert
        partnerBankAccount.BankAccount.Should().Be(bankAccount);
        partnerBankAccount.IsDefault.Should().BeFalse();
    }

    [Fact]
    public void Create_NullBankAccount_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => PartnerBankAccount.Create(null!));
    }

    [Fact]
    public void Reconstitute_ValidParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = 10L;
        var bankAccount = new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX");
        var isDefault = true;
        // Act
        var partnerBankAccount = PartnerBankAccount.Reconstitute(id, bankAccount, isDefault);
        // Assert
        partnerBankAccount.Id.Should().Be(id);
        partnerBankAccount.BankAccount.Should().Be(bankAccount);
        partnerBankAccount.IsDefault.Should().BeTrue();
    }

    [Fact]
    public void Reconstitute__OptionalParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = 10L;
        var bankAccount = new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX");
        // Act
        var partnerBankAccount = PartnerBankAccount.Reconstitute(id, bankAccount);
        // Assert
        partnerBankAccount.Id.Should().Be(id);
        partnerBankAccount.BankAccount.Should().Be(bankAccount);
        partnerBankAccount.IsDefault.Should().BeFalse();
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void Reconstitute_InvalidId_ThrowsArgumentException(long invalidId)
    {
        // Arrange
        var bankAccount = new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX");
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PartnerBankAccount.Reconstitute(invalidId, bankAccount));
    }

    [Fact]
    public void Reconstitute_NullBankAccount_ThrowsArgumentNullException()
    {
        // Arrange
        var id = 10L;
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => PartnerBankAccount.Reconstitute(id, null!));
    }

    [Fact]
    public void UpdateBankAccount_ValidParameters_UpdatesPropertiesCorrectly()
    {
        // Arrange
        var partnerBankAccount = PartnerBankAccount.Create(new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX"));
        var newBankAccount = new BankAccount("New Bank", "DE89370400440532013000", "usd", "NEWBDEFFXXX");
        var isDefault = true;
        // Act
        partnerBankAccount.UpdateBankAccount(newBankAccount, isDefault);
        // Assert
        partnerBankAccount.BankAccount.Should().Be(newBankAccount);
        partnerBankAccount.IsDefault.Should().BeTrue();
    }

    [Fact]
    public void UpdateBankAccount_NullBankAccount_ThrowsArgumentNullException()
    {
        // Arrange
        var partnerBankAccount = PartnerBankAccount.Create(new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX"));
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => partnerBankAccount.UpdateBankAccount(null!, true));
    }
}
