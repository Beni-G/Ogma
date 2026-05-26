using FluentAssertions;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;
using Ogma.Domain.UnitTests.Partners.Helpers;

namespace Ogma.Domain.UnitTests.Partners.Tests;

public class PartnerContactTests
{
    [Fact]
    public void Create_ValidParamteres_SetsPropertiesCorrectly()
    {
        // Arrange
        PersonName name = new PersonName("John", "Doe");
        var email = new Email("abc@mail.com");
        string phone = "1234567890";
        string mobile = "0987654321";
        string title = "Mr.";
        string jobTitle = "Manager";
        bool isPrimary = true;
        // Act
        var contact = PartnerContact.Create(
            name,
            email,
            phone,
            mobile,
            title,
            jobTitle,
            isPrimary);
        // Assert
        contact.Name.Should().BeEquivalentTo(name);
        contact.Email.Should().BeEquivalentTo(email);
        contact.Phone.Should().Be(phone);
        contact.Mobile.Should().Be(mobile);
        contact.Title.Should().Be(title);
        contact.JobTitle.Should().Be(jobTitle);
        contact.IsPrimary.Should().Be(isPrimary);
    }

    [Fact]
    public void Create_NullOptionalParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        PersonName name = new PersonName("John", "Doe");
        // Act
        var contact = PartnerContact.Create(name);
        // Assert
        contact.Name.Should().BeEquivalentTo(name);
        contact.Email.Should().BeNull();
        contact.Phone.Should().BeNull();
        contact.Mobile.Should().BeNull();
        contact.Title.Should().BeNull();
        contact.JobTitle.Should().BeNull();
        contact.IsPrimary.Should().BeFalse();
    }

    [Fact]
    public void Create_NullName_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => PartnerContact.Create(null!));
    }

    [Fact]
    public void Reconstitute_ValidParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        long id = 10;
        PersonName name = new PersonName("John", "Doe");
        var email = new Email("abc@mail.com");
        string phone = "1234567890";
        string mobile = "0987654321";
        string title = "Mr.";
        string jobTitle = "Manager";
        bool isPrimary = true;
        // Act
        var contact = PartnerContact.Reconstitute(
            id,
            name,
            PartnersTestData.GetMetadata(),
            email,
            phone,
            mobile,
            title,
            jobTitle,
            isPrimary);
        // Assert
        contact.Id.Should().Be(id);
        contact.Name.Should().BeEquivalentTo(name);
        contact.Email.Should().BeEquivalentTo(email);
        contact.Phone.Should().Be(phone);
        contact.Mobile.Should().Be(mobile);
        contact.Title.Should().Be(title);
        contact.JobTitle.Should().Be(jobTitle);
        contact.IsPrimary.Should().Be(isPrimary);
    }

    [Fact]
    public void Reconstitute_NullOptionalParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        long id = 10;
        PersonName name = new PersonName("John", "Doe");
        // Act
        var contact = PartnerContact.Reconstitute(id, name, PartnersTestData.GetMetadata());
        // Assert
        contact.Id.Should().Be(id);
        contact.Name.Should().BeEquivalentTo(name);
        contact.Email.Should().BeNull();
        contact.Phone.Should().BeNull();
        contact.Mobile.Should().BeNull();
        contact.Title.Should().BeNull();
        contact.JobTitle.Should().BeNull();
        contact.IsPrimary.Should().BeFalse();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Reconstitute_InvalidId_ThrowsArgumentException(long invalidId)
    {
        // Arrange
        long partnerId = 1;
        PersonName name = new PersonName("John", "Doe");
        // Act & Assert
        Assert.Throws<ArgumentException>(() => PartnerContact.Reconstitute(invalidId, name, PartnersTestData.GetMetadata()));
    }

    [Fact]
    public void Reconstitute_NullName_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => PartnerContact.Reconstitute(1L, null, PartnersTestData.GetMetadata()));
    }

    [Fact]
    public void UpdateDetails_ValidParameters_UpdatesPropertiesCorrectly()
    {
        // Arrange
        var contact = PartnerContact.Create(new PersonName("John", "Doe"));
        var newName = new PersonName("Jane", "Smith");
        var newEmail = new Email("abc@mail.com");
        string newPhone = "1234567890";
        string newMobile = "0987654321";
        string newTitle = "Ms.";
        string newJobTitle = "Director";
        bool newIsPrimary = true;
        // Act
        contact.Update(
            newName,
            newEmail,
            newPhone,
            newMobile,
            newTitle,
            newJobTitle,
            newIsPrimary);
        // Assert
        contact.Name.Should().BeEquivalentTo(newName);
        contact.Email.Should().BeEquivalentTo(newEmail);
        contact.Phone.Should().Be(newPhone);
        contact.Mobile.Should().Be(newMobile);
        contact.Title.Should().Be(newTitle);
        contact.JobTitle.Should().Be(newJobTitle);
        contact.IsPrimary.Should().Be(newIsPrimary);
    }

    [Fact]
    public void UpdateDetails_ValidParameters_UpdatesMetadataCorrectly()
    {
        // Arrange
        var contact = PartnerContact.Create(new PersonName("John", "Doe"));
        var oldContactMetadata = new EntityMetadata(contact.Metadata.CreatedAt, contact.Metadata.UpdatedAt, contact.Metadata.Version);
        var newName = new PersonName("Jane", "Smith");
        var newEmail = new Email("abc@mail.com");
        string newPhone = "1234567890";
        string newMobile = "0987654321";
        string newTitle = "Ms.";
        string newJobTitle = "Director";
        bool newIsPrimary = true;
        // Act
        contact.Update(
            newName,
            newEmail,
            newPhone,
            newMobile,
            newTitle,
            newJobTitle,
            newIsPrimary);
        // Assert
        contact.Metadata.CreatedAt.Should().Be(oldContactMetadata.CreatedAt);
        contact.Metadata.UpdatedAt.Should().BeAfter(oldContactMetadata.UpdatedAt);
        contact.Metadata.Version.Should().Be(oldContactMetadata.Version + 1);
    }

    [Fact]
    public void UpdateDetails_NullOptionalParameters_UpdatesPropertiesCorrectly()
    {
        // Arrange
        var contact = PartnerContact.Create(
            new PersonName("John", "Doe"),
            new Email("abc@mail.com"),
            "1234567890",
            "0987654321",
            "Mr.",
            "Manager",
            true);
        var newName = new PersonName("Jane", "Smith");
        // Act
        contact.Update(newName);
        // Assert
        contact.Name.Should().BeEquivalentTo(newName);
        contact.Email.Should().BeNull();
        contact.Phone.Should().BeNull();
        contact.Mobile.Should().BeNull();
        contact.Title.Should().BeNull();
        contact.JobTitle.Should().BeNull();
        contact.IsPrimary.Should().BeFalse();
    }

    [Fact]
    public void UpdateDetails_NullName_ThrowsArgumentNullException()
    {
        // Arrange
        var contact = PartnerContact.Create(new PersonName("John", "Doe"));
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => contact.Update(null));
    }

}
