using FluentAssertions;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.UnitTests.SharedKernel.ValueObjects.Tests;

public class EmailTests
{
    [Theory]
    [InlineData("John@mail.com")]
    [InlineData("   John@mail.com   ")]
    public void Constructor_ValidEmail_SetsPropertyCorrectly(string emailAddress)
    {
        // Act
        var email = new Email(emailAddress);
        // Assert
        email.Value.Should().Be(emailAddress.Trim().ToLowerInvariant());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("not-an-email")]
    [InlineData("@missing-local.com")]
    [InlineData("missing-at-sign")]
    [InlineData("bad..dots@example.com")]
    public void Constructor_InvalidEmail_ThrowsArgumentException(string invalidEmail)
    {
        Assert.Throws<ArgumentException>(() => new Email(invalidEmail));
    }

    [Fact]
    public void Constructor_EmailTooLong_ThrowsArgumentException()
    {
        // Arrange
        var localPart = new string('a', 246);
        var emailAddress = $"{localPart}@mail.com";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Email(emailAddress));
    }

    [Fact]
    public void Constructor_EmailMaxLength_SuccessfullyCreated()
    {
        // Arrange
        var localPart = new string('a', 245);
        var emailAddress = $"{localPart}@mail.com";
        // Act & Assert
        var emailCreated = new Email(emailAddress);
        // Arrange 
        emailCreated.Value.Should().Be(emailAddress);
    }

}
