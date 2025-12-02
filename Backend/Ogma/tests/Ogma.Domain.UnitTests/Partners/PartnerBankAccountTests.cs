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
        var partnerId = 1L;
        var bankAccount = new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX");
        var isDefault = true;
        // Act
        var partnerBankAccount = PartnerBankAccount.Create(partnerId, bankAccount, isDefault);
        // Assert
        partnerBankAccount.PartnerId.Should().Be(partnerId);
        partnerBankAccount.BankAccount.Should().Be(bankAccount);
        partnerBankAccount.IsDefault.Should().BeTrue();
    }

    [Fact]
    public void Create__OptionalParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var partnerId = 1L;
        var bankAccount = new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX");
        // Act
        var partnerBankAccount = PartnerBankAccount.Create(partnerId, bankAccount);
        // Assert
        partnerBankAccount.PartnerId.Should().Be(partnerId);
        partnerBankAccount.BankAccount.Should().Be(bankAccount);
        partnerBankAccount.IsDefault.Should().BeFalse();
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void Create_InvalidPartnerId_ThrowsArgumentException(long invalidPartnerId)
    {
        // Arrange
        var bankAccount = new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX");
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PartnerBankAccount.Create(invalidPartnerId, bankAccount));
    }

    [Fact]
    public void Create_NullBankAccount_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => PartnerBankAccount.Create(1L, null!));
    }

    [Fact]
    public void Reconstitute_ValidParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = 10L;
        var partnerId = 1L;
        var bankAccount = new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX");
        var isDefault = true;
        // Act
        var partnerBankAccount = PartnerBankAccount.Reconstitute(id, partnerId, bankAccount, isDefault);
        // Assert
        partnerBankAccount.Id.Should().Be(id);
        partnerBankAccount.PartnerId.Should().Be(partnerId);
        partnerBankAccount.BankAccount.Should().Be(bankAccount);
        partnerBankAccount.IsDefault.Should().BeTrue();
    }

    [Fact]
    public void Reconstitute__OptionalParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = 10L;
        var partnerId = 1L;
        var bankAccount = new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX");
        // Act
        var partnerBankAccount = PartnerBankAccount.Reconstitute(id, partnerId, bankAccount);
        // Assert
        partnerBankAccount.Id.Should().Be(id);
        partnerBankAccount.PartnerId.Should().Be(partnerId);
        partnerBankAccount.BankAccount.Should().Be(bankAccount);
        partnerBankAccount.IsDefault.Should().BeFalse();
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void Reconstitute_InvalidId_ThrowsArgumentException(long invalidId)
    {
        // Arrange
        var partnerId = 1L;
        var bankAccount = new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX");
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PartnerBankAccount.Reconstitute(invalidId, partnerId, bankAccount));
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void Reconstitute_InvalidPartnerId_ThrowsArgumentException(long invalidPartnerId)
    {
        // Arrange
        var id = 10L;
        var bankAccount = new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX");
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PartnerBankAccount.Reconstitute(id, invalidPartnerId, bankAccount));
    }

    [Fact]
    public void Reconstitute_NullBankAccount_ThrowsArgumentNullException()
    {
        // Arrange
        var id = 10L;
        var partnerId = 1L;
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => PartnerBankAccount.Reconstitute(id, partnerId, null!));
    }

    [Fact]
    public void UpdateBankAccount_ValidParameters_UpdatesPropertiesCorrectly()
    {
        // Arrange
        var partnerBankAccount = PartnerBankAccount.Create(1L, new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX"));
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
        var partnerBankAccount = PartnerBankAccount.Create(1L, new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX"));
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => partnerBankAccount.UpdateBankAccount(null!, true));
    }
}
