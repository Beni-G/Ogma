using FluentAssertions;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.UnitTests.Partners.Helpers;

namespace Ogma.Domain.UnitTests.Partners.Tests;

public class PartnerRoleTypeTests
{
    [Fact]
    public void CreatePartnerRole_ValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var code = "CUSTOMER";
        var name = "Customer";
        var color = "#FF5733";
        // Act
        var partnerRole = PartnerRoleType.Create(code, name, color);
        // Assert
        partnerRole.Code.Should().Be(code);
        partnerRole.Name.Should().Be(name);
        partnerRole.Color.Should().Be(color);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreatePartnerRole_NullOrEmptyCode_ShouldThrowArgumentException(string invalidCode)
    {
        // Arrange
        var name = "Customer";
        // Act
        Action act = () => PartnerRoleType.Create(invalidCode, name);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreatePartnerRole_NullOrEmptyName_ShouldThrowArgumentException(string invalidName)
    {
        // Arrange
        var code = "CUSTOMER";
        // Act
        Action act = () => PartnerRoleType.Create(code, invalidName);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreatePartnerRole_NullColor_ShouldCreateInstanceWithNullColor()
    {
        // Arrange
        var code = "SUPPLIER";
        var name = "Supplier";
        // Act
        var partnerRole = PartnerRoleType.Create(code, name);
        // Assert
        partnerRole.Code.Should().Be(code);
        partnerRole.Name.Should().Be(name);
        partnerRole.Color.Should().BeNull();
    }

    [Fact]
    public void ReconstitutePartnerRole_ValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var id = 1L;
        var code = "DISTRIBUTOR";
        var name = "Distributor";
        var color = "#33FF57";
        // Act
        var partnerRole = PartnerRoleType.Reconstitute(id, code, name, PartnersTestData.GetMetadata(), color);
        // Assert
        partnerRole.Id.Should().Be(id);
        partnerRole.Code.Should().Be(code);
        partnerRole.Name.Should().Be(name);
        partnerRole.Color.Should().Be(color);
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void ReconstitutePartnerRole_NonPositiveId_ShouldThrowArgumentException(long invalidId)
    {
        // Arrange
        var code = "RESELLER";
        var name = "Reseller";
        // Act
        Action act = () => PartnerRoleType.Reconstitute(invalidId, code, name, PartnersTestData.GetMetadata());
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ReconstitutePartnerRole_NullOrEmptyCode_ShouldThrowArgumentException(string invalidCode)
    {
        // Arrange
        var id = 1L;
        var name = "Reseller";
        // Act
        Action act = () => PartnerRoleType.Reconstitute(id, invalidCode, name, PartnersTestData.GetMetadata());
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ReconstitutePartnerRole_NullOrEmptyName_ShouldThrowArgumentException(string invalidName)
    {
        // Arrange
        var id = 1L;
        var code = "RESELLER";
        // Act
        Action act = () => PartnerRoleType.Reconstitute(id, code, invalidName, PartnersTestData.GetMetadata());
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ReconstitutePartnerRole_NullColor_ShouldCreateInstanceWithNullColor()
    {
        // Arrange
        var id = 2L;
        var code = "PARTNER";
        var name = "Partner";
        // Act
        var partnerRole = PartnerRoleType.Reconstitute(id, code, name, PartnersTestData.GetMetadata());
        // Assert
        partnerRole.Id.Should().Be(id);
        partnerRole.Code.Should().Be(code);
        partnerRole.Name.Should().Be(name);
        partnerRole.Color.Should().BeNull();
    }

    [Fact]
    public void Update_ValidParameters_ShouldUpdateProperties()
    {
        // Arrange
        var partnerRole = PartnerRoleType.Create("OLD_CODE", "Old Name", "#000000");
        var newCode = "NEW_CODE";
        var newName = "New Name";
        var newColor = "#FFFFFF";
        // Act
        partnerRole.Update(newCode, newName, newColor);
        // Assert
        partnerRole.Code.Should().Be(newCode);
        partnerRole.Name.Should().Be(newName);
        partnerRole.Color.Should().Be(newColor);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_NullOrEmptyCode_ShouldThrowArgumentException(string invalidCode)
    {
        // Arrange
        var partnerRole = PartnerRoleType.Create("VALID_CODE", "Valid Name");
        var newName = "New Name";
        // Act
        Action act = () => partnerRole.Update(invalidCode, newName);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_NullOrEmptyName_ShouldThrowArgumentException(string invalidName)
    {
        // Arrange
        var partnerRole = PartnerRoleType.Create("VALID_CODE", "Valid Name");
        var newCode = "New_Code";
        // Act
        Action act = () => partnerRole.Update(newCode, invalidName);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_NullColor_ShouldSetColorToNull()
    {
        // Arrange
        var partnerRole = PartnerRoleType.Create("VALID_CODE", "Valid Name", "#123456");
        var newCode = "UPDATED_CODE";
        var newName = "Updated Name";
        // Act
        partnerRole.Update(newCode, newName, null);
        // Assert
        partnerRole.Code.Should().Be(newCode);
        partnerRole.Name.Should().Be(newName);
        partnerRole.Color.Should().BeNull();
    }
}
