using FluentAssertions;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.UnitTests.Catalog.Helpers;

namespace Ogma.Domain.UnitTests.Catalog.Tests;

public class ItemTypeTests
{
    [Fact]
    public void Create_ValidNameAndDescription_SetsPropertiesCorrectly()
    {
        // Arrange
        string name = "TestItem";
        string description = "A test item type";

        // Act
        var itemType = ItemType.Create(name, description);

        // Assert
        itemType.Name.Should().Be(name);
        itemType.Description.Should().Be(description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmptyName_ThrowsArgumentException(string invalidName)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => ItemType.Create(invalidName, "Description"));
    }

    [Fact]
    public void Create_NullDescription_SetsDescriptionCorrectly()
    {
        // Arrange
        string name = "TestItem";

        // Act
        var itemType = ItemType.Create(name, null);

        // Assert
        itemType.Name.Should().Be(name);
        itemType.Description.Should().BeNull();
    }

    [Fact]
    public void Reconstitute_ValidNameAndDescription_SetsPropertiesCorrectly()
    {
        // Arrange
        string name = "TestItem";
        string description = "A test item type";
        long id = 1;

        // Act
        var itemType = ItemType.Reconstitute(id, name, description, CatalogTestData.GetMetadata());

        // Assert
        itemType.Id.Should().Be(id);
        itemType.Name.Should().Be(name);
        itemType.Description.Should().Be(description);
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void Reconstitute_NonPositiveId_ThrowsArgumentException(long invalidId)
    {
        // Arrange
        string name = "TestItem";
        string description = "A test item type";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => ItemType.Reconstitute(invalidId, name, description, CatalogTestData.GetMetadata()));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Reconstitute_NullOrEmptyName_ThrowsArgumentException(string invalidName)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => ItemType.Reconstitute(1, invalidName, "Description", CatalogTestData.GetMetadata()));
    }

    [Fact]
    public void Reconstitute_NullDescription_SetsDescriptionCorrectly()
    {
        // Arrange
        long id = 1;
        string name = "TestItem";

        // Act
        var itemType = ItemType.Reconstitute(id, name, null, CatalogTestData.GetMetadata());

        // Assert
        itemType.Id.Should().Be(id);
        itemType.Name.Should().Be(name);
        itemType.Description.Should().BeNull();
    }

    [Fact]
    public void Update_ValidNameAndDescription_UpdatesPropertiesCorrectly()
    {
        // Arrange
        var itemType = ItemType.Create("OldName", "OldDescription");
        string newName = "NewName";
        string newDescription = "NewDescription";

        // Act
        itemType.Update(newName, newDescription);

        // Assert
        itemType.Name.Should().Be(newName);
        itemType.Description.Should().Be(newDescription);
    }

    [Fact]
    public void Update_ValidNameAndDescription_UpdatesMetadataCorrectly()
    {
        // Arrange
        var itemType = ItemType.Create("OldName", "OldDescription");
        var oldItemTypeMetadata = new EntityMetadata(itemType.Metadata.CreatedAt, itemType.Metadata.UpdatedAt, itemType.Metadata.Version);
        string newName = "NewName";
        string newDescription = "NewDescription";

        // Act
        itemType.Update(newName, newDescription);

        // Assert
        itemType.Metadata.CreatedAt.Should().Be(oldItemTypeMetadata.CreatedAt);
        itemType.Metadata.UpdatedAt.Should().BeAfter(oldItemTypeMetadata.UpdatedAt);
        itemType.Metadata.Version.Should().Be(oldItemTypeMetadata.Version + 1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_NullOrEmptyName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var itemType = ItemType.Create("OldName", "OldDescription");
        // Act & Assert
        Assert.Throws<ArgumentException>(() => itemType.Update(invalidName, "NewDescription"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_NullOrEmptyDescription_UpdatesDescriptionCorrectly(string newDescription)
    {
        // Arrange
        var itemType = ItemType.Create("OldName", "OldDescription");
        string newName = "NewName";

        // Act
        itemType.Update(newName, newDescription);

        // Assert
        itemType.Name.Should().Be(newName);
        itemType.Description.Should().Be(newDescription);
    }

    [Fact]
    public void Reconstitute_ValidId_RetainsId()
    {
        // Arrange
        long id = 42;
        string name = "TestItem";
        string description = "A test item type";
        var itemType = ItemType.Reconstitute(id, name, description, CatalogTestData.GetMetadata());

        // Act
        itemType.Update("UpdatedName", "UpdatedDescription");

        // Assert
        itemType.Id.Should().Be(id);
    }

    [Fact]
    public void Update_MultipleUpdates_MaintainsValidNameAndDescription()
    {
        // Arrange
        var itemType = ItemType.Create("InitialName", "InitialDescription");

        // Act
        itemType.Update("SecondName", "SecondDescription");
        itemType.Update("ThirdName", "ThirdDescription");

        // Assert
        itemType.Name.Should().Be("ThirdName");
        itemType.Description.Should().Be("ThirdDescription");
    }
}
