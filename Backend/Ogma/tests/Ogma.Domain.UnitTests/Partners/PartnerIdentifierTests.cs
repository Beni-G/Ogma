using FluentAssertions;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.UnitTests.Partners;

public class PartnerIdentifierTests
{
    [Fact]
    public void Create_ValidParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        long partnerId = 1;
        string type = "TaxID";
        string value = "123-45-6789";
        var validityPeriod = new Period(DateTime.UtcNow, DateTime.UtcNow.AddYears(1));
        bool isPrimary = true;
        // Act
        var partnerIdentifier = PartnerIdentifier.Create(partnerId, type, value, validityPeriod, isPrimary);
        // Assert
        partnerIdentifier.PartnerId.Should().Be(partnerId);
        partnerIdentifier.Type.Should().Be(type);
        partnerIdentifier.Value.Should().Be(value);
        partnerIdentifier.ValidityPeriod.Should().Be(validityPeriod);
        partnerIdentifier.IsPrimary.Should().Be(isPrimary);
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void Create_InvalidPartnerId_ThrowsArgumentException(long invalidPartnerId)
    {
        // Arrange
        string type = "TaxID";
        string value = "123-45-6789";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PartnerIdentifier.Create(invalidPartnerId, type, value));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmptyType_ThrowsArgumentException(string invalidType)
    {
        // Arrange
        long partnerId = 1;
        string value = "123-45-6789";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PartnerIdentifier.Create(partnerId, invalidType, value));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmptyValue_ThrowsArgumentException(string invalidValue)
    {
        // Arrange
        long partnerId = 1;
        string type = "TaxID";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PartnerIdentifier.Create(partnerId, type, invalidValue));
    }

    [Fact]
    public void Create_WithoutValidityPeriod_SetsPropertiesCorrectly()
    {
        // Arrange
        long partnerId = 1;
        string type = "TaxID";
        string value = "123-45-6789";
        // Act
        var partnerIdentifier = PartnerIdentifier.Create(partnerId, type, value);
        // Assert
        partnerIdentifier.PartnerId.Should().Be(partnerId);
        partnerIdentifier.Type.Should().Be(type);
        partnerIdentifier.Value.Should().Be(value);
        partnerIdentifier.ValidityPeriod.Should().BeNull();
        partnerIdentifier.IsPrimary.Should().BeFalse();
    }

    [Fact]
    public void Create_WithoutIsPrimary_SetsIsPrimaryToFalse()
    {
        // Arrange
        long partnerId = 1;
        string type = "TaxID";
        string value = "123-45-6789";
        var validityPeriod = new Period(DateTime.UtcNow, DateTime.UtcNow.AddYears(1));
        // Act
        var partnerIdentifier = PartnerIdentifier.Create(partnerId, type, value, validityPeriod);
        // Assert
        partnerIdentifier.IsPrimary.Should().BeFalse();
    }

    [Fact]
    public void Reconstitute_ValidParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        long id = 1;
        long partnerId = 1;
        string type = "TaxID";
        string value = "123-45-6789";
        var validityPeriod = new Period(DateTime.UtcNow, DateTime.UtcNow.AddYears(1));
        bool isPrimary = true;
        // Act
        var partnerIdentifier = PartnerIdentifier.Reconstitute(id, partnerId, type, value, validityPeriod, isPrimary);
        // Assert
        partnerIdentifier.Id.Should().Be(id);
        partnerIdentifier.PartnerId.Should().Be(partnerId);
        partnerIdentifier.Type.Should().Be(type);
        partnerIdentifier.Value.Should().Be(value);
        partnerIdentifier.ValidityPeriod.Should().Be(validityPeriod);
        partnerIdentifier.IsPrimary.Should().Be(isPrimary);
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void Reconstitute_InvalidId_ThrowsArgumentException(long invalidId)
    {
        // Arrange
        long partnerId = 1;
        string type = "TaxID";
        string value = "123-45-6789";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PartnerIdentifier.Reconstitute(invalidId, partnerId, type, value));
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void Reconstitute_InvalidPartnerId_ThrowsArgumentException(long invalidPartnerId)
    {
        // Arrange
        long id = 1;
        string type = "TaxID";
        string value = "123-45-6789";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PartnerIdentifier.Reconstitute(id, invalidPartnerId, type, value));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Reconstitute_NullOrEmptyType_ThrowsArgumentException(string invalidType)
    {
        // Arrange
        long id = 1;
        long partnerId = 1;
        string value = "123-45-6789";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PartnerIdentifier.Reconstitute(id, partnerId, invalidType, value));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Reconstitute_NullOrEmptyValue_ThrowsArgumentException(string invalidValue)
    {
        // Arrange
        long id = 1;
        long partnerId = 1;
        string type = "TaxID";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PartnerIdentifier.Reconstitute(id, partnerId, type, invalidValue));
    }


    [Fact]
    public void Reconstitute_WithoutValidityPeriod_SetsPropertiesCorrectly()
    {
        // Arrange
        long id = 1;
        long partnerId = 1;
        string type = "TaxID";
        string value = "123-45-6789";
        // Act
        var partnerIdentifier = PartnerIdentifier.Reconstitute(id, partnerId, type, value);
        // Assert
        partnerIdentifier.Id.Should().Be(id);
        partnerIdentifier.PartnerId.Should().Be(partnerId);
        partnerIdentifier.Type.Should().Be(type);
        partnerIdentifier.Value.Should().Be(value);
        partnerIdentifier.ValidityPeriod.Should().BeNull();
        partnerIdentifier.IsPrimary.Should().BeFalse();
    }

    [Fact]
    public void Reconstitute_WithoutIsPrimary_SetsIsPrimaryToFalse()
    {
        // Arrange
        long id = 1;
        long partnerId = 1;
        string type = "TaxID";
        string value = "123-45-6789";
        var validityPeriod = new Period(DateTime.UtcNow, DateTime.UtcNow.AddYears(1));
        // Act
        var partnerIdentifier = PartnerIdentifier.Reconstitute(id, partnerId, type, value, validityPeriod);
        // Assert
        partnerIdentifier.IsPrimary.Should().BeFalse();
    }

    [Fact]
    public void Update_ValidParameters_UpdatesPropertiesCorrectly()
    {
        // Arrange
        var partnerIdentifier = PartnerIdentifier.Create(1, "TaxID", "123-45-6789");
        string newType = "BusinessID";
        string newValue = "987-65-4321";
        var newValidityPeriod = new Period(DateTime.UtcNow, DateTime.UtcNow.AddYears(2));
        bool newIsPrimary = true;
        // Act
        partnerIdentifier.Update(newType, newValue, newValidityPeriod, newIsPrimary);
        // Assert
        partnerIdentifier.Type.Should().Be(newType);
        partnerIdentifier.Value.Should().Be(newValue);
        partnerIdentifier.ValidityPeriod.Should().Be(newValidityPeriod);
        partnerIdentifier.IsPrimary.Should().Be(newIsPrimary);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_NullOrEmptyType_ThrowsArgumentException(string invalidType)
    {
        // Arrange
        var partnerIdentifier = PartnerIdentifier.Create(1, "TaxID", "123-45-6789");
        string newValue = "987-65-4321";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => partnerIdentifier.Update(invalidType, newValue));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_NullOrEmptyValue_ThrowsArgumentException(string invalidValue)
    {
        // Arrange
        var partnerIdentifier = PartnerIdentifier.Create(1, "TaxID", "123-45-6789");
        string newType = "BusinessID";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => partnerIdentifier.Update(newType, invalidValue));
    }

    [Fact]
    public void Update_WithoutValidityPeriod_SetsValidityPeriodToNull()
    {
        // Arrange
        var partnerIdentifier = PartnerIdentifier.Create(1, "TaxID", "123-45-6789", new Period(DateTime.UtcNow, DateTime.UtcNow.AddYears(1)));
        string newType = "BusinessID";
        string newValue = "987-65-4321";
        // Act
        partnerIdentifier.Update(newType, newValue);
        // Assert
        partnerIdentifier.ValidityPeriod.Should().BeNull();
    }

    [Fact]
    public void Update_WithoutIsPrimary_SetsIsPrimaryToFalse()
    {
        // Arrange
        var partnerIdentifier = PartnerIdentifier.Create(1, "TaxID", "123-45-6789", isPrimary: true);
        string newType = "BusinessID";
        string newValue = "987-65-4321";
        var newValidityPeriod = new Period(DateTime.UtcNow, DateTime.UtcNow.AddYears(2));
        // Act
        partnerIdentifier.Update(newType, newValue, newValidityPeriod);
        // Assert
        partnerIdentifier.IsPrimary.Should().BeFalse();
    }

    [Fact]
    public void MarkAsPrimary_SetsIsPrimaryToTrue()
    {
        // Arrange
        var partnerIdentifier = PartnerIdentifier.Create(1, "TaxID", "123-45-6789");
        // Act
        partnerIdentifier.MarkAsPrimary();
        // Assert
        partnerIdentifier.IsPrimary.Should().BeTrue();
    }

    [Fact]
    public void UnmarkAsPrimary_SetsIsPrimaryToTrue()
    {
        // Arrange
        var partnerIdentifier = PartnerIdentifier.Create(1, "TaxID", "123-45-6789", isPrimary:true);
        // Act
        partnerIdentifier.UnmarkAsPrimary();
        // Assert
        partnerIdentifier.IsPrimary.Should().BeFalse();
    }

}
