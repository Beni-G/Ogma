using FluentAssertions;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.UnitTests.SharedKernel.ValueObjects;

public class PersonNameTests
{
    [Fact]
    public void Constructor_ValidParameters_SetPropertiesCorrectly()
    {
        // Arrange
        var firstName = "John ";
        var lastName = " Doe ";

        // Act
        var personName = new PersonName(firstName, lastName);

        // Assert
        personName.Should().NotBeNull();
        personName.FirstName.Should().Be(firstName.Trim());
        personName.LastName.Should().Be(lastName.Trim());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidFirstName_ThrowsArgumentException(string invalidFirstName)
    {
        // Arrange
        var lastName = "Doe";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new PersonName(invalidFirstName, lastName));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidLastName_ThrowsArgumentException(string invalidLastName)
    {
        // Arrange
        var firstName = "John";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new PersonName(firstName, invalidLastName));
    }

    [Fact]
    public void FullName_ReturnsFullNameCorrectly()
    {
        // Arrange
        var firstName = "John ";
        var lastName = " Doe ";

        // Act
        var personName = new PersonName(firstName, lastName);

        // Assert
        personName.FullName.Should().Be($"{firstName.Trim()} {lastName.Trim()}");
    }
}
