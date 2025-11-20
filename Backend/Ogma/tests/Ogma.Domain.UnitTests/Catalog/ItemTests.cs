using FluentAssertions;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Parameters;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.UnitTests.Catalog;
public class ItemTests
{
    [Fact]
    public void Create_ValidAllParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var itemParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece",
            IsActive: true,
            Description: "A high-end smartphone"
        );
        // Act
        var item = Item.Create(itemParameters);

        // Assert
        item.Name.Should().Be(itemParameters.Name);
        item.Code.Should().Be(itemParameters.Code);
        item.Description.Should().Be(itemParameters.Description);
        item.Category.Should().Be(itemParameters.Category);
        item.ListPrice.Should().Be(itemParameters.ListPrice);
        item.ItemType.Should().Be(itemParameters.ItemType);
        item.UnitOfMeasurement.Should().Be(itemParameters.UnitOfMeasurement);
        item.IsActive.Should().Be(itemParameters.IsActive);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmptyName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var itemParameters = new ItemParameters(
            Name: invalidName,
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece",
            IsActive: true,
            Description: "A high-end smartphone"
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Create(itemParameters));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmptyCode_ThrowsArgumentException(string invalidCode)
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var itemParameters = new ItemParameters(
            Name: "Smartphone",
            Code: invalidCode,
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece",
            IsActive: true,
            Description: "A high-end smartphone"
        );
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Create(itemParameters));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmptyUnitOfMeasurement_ThrowsArgumentException(string invalidUom)
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var itemParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: invalidUom,
            IsActive: true,
            Description: "A high-end smartphone"
        );
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Create(itemParameters));
    }

    [Fact]
    public void Create_NullItemParameters_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Item.Create(null));
    }

    [Fact]
    public void Create_ValidOnlyMandatoryParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var itemParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );

        // Act
        var item = Item.Create(itemParameters);

        // Assert
        item.Name.Should().Be(itemParameters.Name);
        item.Code.Should().Be(itemParameters.Code);
        item.Description.Should().Be(string.Empty);
        item.Category.Should().Be(itemParameters.Category);
        item.ListPrice.Should().Be(itemParameters.ListPrice);
        item.ItemType.Should().Be(itemParameters.ItemType);
        item.UnitOfMeasurement.Should().Be(itemParameters.UnitOfMeasurement);
        item.IsActive.Should().Be(true);
    }

    [Fact]
    public void Create_NullCategory_ThrowsArgumentException()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var listPrice = new Money(99.99m, "USD");
        var itemParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: null,
            UnitOfMeasurement: "Piece"
        );

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Item.Create(itemParameters));
    }

    [Fact]
    public void Create_NullItemType_ThrowsArgumentException()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var listPrice = new Money(99.99m, "USD");
        var itemParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: null,
            UnitOfMeasurement: "Piece"
        );

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Item.Create(itemParameters));
    }

    [Fact]
    public void Create_NullMoney_ThrowsArgumentException()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var itemParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: null,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Item.Create(itemParameters));
    }

    [Fact]
    public void Reconstitute_ValidAllParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = 1L;
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var itemParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece",
            IsActive: true,
            Description: "A high-end smartphone"
        );
        // Act
        var item = Item.Reconstitute(id, itemParameters);

        // Assert
        item.Id.Should().Be(id);
        item.Name.Should().Be(itemParameters.Name);
        item.Code.Should().Be(itemParameters.Code);
        item.Description.Should().Be(itemParameters.Description);
        item.Category.Should().Be(itemParameters.Category);
        item.ListPrice.Should().Be(itemParameters.ListPrice);
        item.ItemType.Should().Be(itemParameters.ItemType);
        item.UnitOfMeasurement.Should().Be(itemParameters.UnitOfMeasurement);
        item.IsActive.Should().Be(itemParameters.IsActive);
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void Reconstitute_InvalidId_ThrowsArgumentException(long invalidId)
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var itemParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece",
            IsActive: true,
            Description: "A high-end smartphone"
        );
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Reconstitute(invalidId, itemParameters));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Reconstitute_NullOrEmptyName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var id = 1L;
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var itemParameters = new ItemParameters(
            Name: invalidName,
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece",
            IsActive: true,
            Description: "A high-end smartphone"
        );

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Reconstitute(id, itemParameters));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Reconstitute_NullOrEmptyCode_ThrowsArgumentException(string invalidCode)
    {
        // Arrange
        var id = 1L;
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var itemParameters = new ItemParameters(
            Name: "Smartphone",
            Code: invalidCode,
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece",
            IsActive: true,
            Description: "A high-end smartphone"
        );
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Reconstitute(id, itemParameters));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Reconstitute_NullOrEmptyUnitOfMeasurement_ThrowsArgumentException(string invalidUom)
    {
        // Arrange
        var id = 1L;
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var itemParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: invalidUom,
            IsActive: true,
            Description: "A high-end smartphone"
        );
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Reconstitute(id, itemParameters));
    }

    [Fact]
    public void Reconstitute_NullItemParameters_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Item.Reconstitute(1L, null));
    }

    [Fact]
    public void Reconstitute_ValidOnlyMandatoryParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = 1L;
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var itemParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );

        // Act
        var item = Item.Reconstitute(id, itemParameters);

        // Assert
        item.Id.Should().Be(id);
        item.Name.Should().Be(itemParameters.Name);
        item.Code.Should().Be(itemParameters.Code);
        item.Description.Should().Be(string.Empty);
        item.Category.Should().Be(itemParameters.Category);
        item.ListPrice.Should().Be(itemParameters.ListPrice);
        item.ItemType.Should().Be(itemParameters.ItemType);
        item.UnitOfMeasurement.Should().Be(itemParameters.UnitOfMeasurement);
        item.IsActive.Should().Be(true);
    }

    [Fact]
    public void Reconstitute_NullCategory_ThrowsArgumentException()
    {
        // Arrange
        var id = 1L;
        var category = Category.Create("Electronics");
        var listPrice = new Money(99.99m, "USD");
        var itemParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: null,
            UnitOfMeasurement: "Piece"
        );

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Item.Reconstitute(id, itemParameters));
    }

    [Fact]
    public void Reconstitute_NullItemType_ThrowsArgumentException()
    {
        // Arrange
        var id = 1L;
        var category = Category.Create("Electronics");
        var listPrice = new Money(99.99m, "USD");
        var itemParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: null,
            UnitOfMeasurement: "Piece"
        );

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Item.Reconstitute(id, itemParameters));
    }

    [Fact]
    public void Reconstitute_NullMoney_ThrowsArgumentException()
    {
        // Arrange
        var id = 1L;
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var itemParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: null,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Item.Reconstitute(id, itemParameters));
    }

    [Fact]
    public void UpdateItem_ValidParameters_UpdatesPropertiesCorrectly()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        var newCategory = Category.Create("Home Appliances");
        var newItemType = ItemType.Create("Appliance", "A useful appliance");
        var newListPrice = new Money(149.99m, "USD");
        var updatedParameters = new ItemParameters(
            Name: "Washing Machine",
            Code: "WM001",
            Category: newCategory,
            ListPrice: newListPrice,
            ItemType: newItemType,
            UnitOfMeasurement: "Unit",
            IsActive: false,
            Description: "A high-efficiency washing machine"
        );
        // Act
        item.UpdateItem(updatedParameters);
        // Assert
        item.Name.Should().Be(updatedParameters.Name);
        item.Code.Should().Be(updatedParameters.Code);
        item.Description.Should().Be(updatedParameters.Description);
        item.Category.Should().Be(updatedParameters.Category);
        item.ListPrice.Should().Be(updatedParameters.ListPrice);
        item.ItemType.Should().Be(updatedParameters.ItemType);
        item.UnitOfMeasurement.Should().Be(updatedParameters.UnitOfMeasurement);
        item.IsActive.Should().Be(updatedParameters.IsActive);
    }

    [Fact]
    public void UpdateItem_NullItemParameters_ThrowsArgumentNullException()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => item.UpdateItem(null));
    }

    [Fact]
    public void UpdateItem_NullCategory_ThrowsArgumentNullException()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        var updatedParameters = new ItemParameters(
            Name: "Washing Machine",
            Code: "WM001",
            Category: null,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Unit"
        );
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => item.UpdateItem(updatedParameters));
    }

    [Fact]
    public void UpdateItem_NullItemType_ThrowsArgumentNullException()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        var updatedParameters = new ItemParameters(
            Name: "Washing Machine",
            Code: "WM001",
            Category: category,
            ListPrice: listPrice,
            ItemType: null,
            UnitOfMeasurement: "Unit"
        );
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => item.UpdateItem(updatedParameters));
    }

    [Fact]
    public void UpdateItem_NullMoney_ThrowsArgumentNullException()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        var updatedParameters = new ItemParameters(
            Name: "Washing Machine",
            Code: "WM001",
            Category: category,
            ListPrice: null,
            ItemType: itemType,
            UnitOfMeasurement: "Unit"
        );
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => item.UpdateItem(updatedParameters));
    }

    [Fact]
    public void UpdateItem_SameValues_DoesNotChangeProperties()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        // Act
        item.UpdateItem(initialParameters);
        // Assert
        item.Name.Should().Be(initialParameters.Name);
        item.Code.Should().Be(initialParameters.Code);
        item.Description.Should().Be(initialParameters.Description);
        item.Category.Should().Be(initialParameters.Category);
        item.ListPrice.Should().Be(initialParameters.ListPrice);
        item.ItemType.Should().Be(initialParameters.ItemType);
        item.UnitOfMeasurement.Should().Be(initialParameters.UnitOfMeasurement);
        item.IsActive.Should().Be(initialParameters.IsActive);
    }

    [Fact]
    public void UpdateItem_ActivateItem_SetsIsActiveToTrue()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece",
            IsActive: false
        );
        var item = Item.Create(initialParameters);
        var updatedParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece",
            IsActive: true
        );
        // Act
        item.UpdateItem(updatedParameters);
        // Assert
        item.IsActive.Should().BeTrue();
    }

    [Fact]
    public void UpdateItem_DeactivateItem_SetsIsActiveToFalse()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece",
            IsActive: true
        );
        var item = Item.Create(initialParameters);
        var updatedParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece",
            IsActive: false
        );
        // Act
        item.UpdateItem(updatedParameters);
        // Assert
        item.IsActive.Should().BeFalse();
    }

    [Fact]
    public void UpdateName_ValidName_UpdatesNameCorrectly()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        var newName = "Super Smartphone";
        // Act
        item.UpdateName(newName);
        // Assert
        item.Name.Should().Be(newName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateName_NullOrEmptyName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        // Act & Assert
        Assert.Throws<ArgumentException>(() => item.UpdateName(invalidName));
    }

    [Fact]
    public void UpdateCode_ValidCode_UpdatesCodeCorrectly()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        var newCode = "SP002";
        // Act
        item.UpdateCode(newCode);
        // Assert
        item.Code.Should().Be(newCode);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateCode_NullOrEmptyCode_ThrowsArgumentException(string invalidCode)
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        // Act & Assert
        Assert.Throws<ArgumentException>(() => item.UpdateCode(invalidCode));
    }

    [Fact]
    public void UpdateDescription_ValidDescription_UpdatesDescriptionCorrectly()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        var newDescription = "An updated description for the smartphone.";
        // Act
        item.UpdateDescription(newDescription);
        // Assert
        item.Description.Should().Be(newDescription);
    }

    [Fact]
    public void UpdateCategory_ValidCategory_UpdatesCategoryCorrectly()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        var newCategory = Category.Create("Mobile Devices");
        // Act
        item.UpdateCategory(newCategory);
        // Assert
        item.Category.Should().Be(newCategory);
    }

    [Fact]
    public void UpdateCategory_NullCategory_ThrowsArgumentNullException()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => item.UpdateCategory(null));
    }

    [Fact]
    public void UpdateListPrice_ValidMoney_UpdatesListPriceCorrectly()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        var newListPrice = new Money(149.99m, "USD");
        // Act
        item.UpdateListPrice(newListPrice);
        // Assert
        item.ListPrice.Should().Be(newListPrice);
    }

    [Fact]
    public void UpdateListPrice_NullMoney_ThrowsArgumentNullException()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => item.UpdateListPrice(null));
    }

    [Fact]
    public void UpdateItemType_ValidItemType_UpdatesItemTypeCorrectly()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        var newItemType = ItemType.Create("Device", "A useful device");
        // Act
        item.UpdateItemType(newItemType);
        // Assert
        item.ItemType.Should().Be(newItemType);
    }

    [Fact]
    public void UpdateItemType_NullItemType_ThrowsArgumentNullException()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => item.UpdateItemType(null));
    }

    [Fact]
    public void UpdateUnitOfMeasurement_ValidUom_UpdatesUnitOfMeasurementCorrectly()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        var newUom = "Box";
        // Act
        item.UpdateUnitOfMeasurement(newUom);
        // Assert
        item.UnitOfMeasurement.Should().Be(newUom);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateUnitOfMeasurement_NullOrEmptyUom_ThrowsArgumentException(string invalidUom)
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece"
        );
        var item = Item.Create(initialParameters);
        // Act & Assert
        Assert.Throws<ArgumentException>(() => item.UpdateUnitOfMeasurement(invalidUom));
    }

    [Fact]
    public void Activate_ItemIsInactive_SetsIsActiveToTrue()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece",
            IsActive: false
        );
        var item = Item.Create(initialParameters);
        // Act
        item.Activate();
        // Assert
        item.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Deactivate_ItemIsActive_SetsIsActiveToFalse()
    {
        // Arrange
        var category = Category.Create("Electronics");
        var itemType = ItemType.Create("Gadget", "A cool gadget");
        var listPrice = new Money(99.99m, "USD");
        var initialParameters = new ItemParameters(
            Name: "Smartphone",
            Code: "SP001",
            Category: category,
            ListPrice: listPrice,
            ItemType: itemType,
            UnitOfMeasurement: "Piece",
            IsActive: true
        );
        var item = Item.Create(initialParameters);
        // Act
        item.Deactivate();
        // Assert
        item.IsActive.Should().BeFalse();
    }
}
