using FluentAssertions;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Parameters;
using Ogma.Domain.SharedKernel.ValueObjects;
using Ogma.Domain.UnitTests.Catalog.Helpers;

namespace Ogma.Domain.UnitTests.Catalog.Tests;
public class ItemTests
{
    [Fact]
    public void Create_ValidAllParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var itemParameters = CatalogTestData.CreateItemParameters();
        // Act
        var item = Item.Create(itemParameters);
        // Assert
        item.Name.Should().Be(itemParameters.Name);
        item.Code.Should().Be(itemParameters.Code);
        item.Description.Should().Be(itemParameters.Description);
        item.CategoryId.Should().Be(itemParameters.CategoryId);
        item.ListPrice.Should().Be(itemParameters.ListPrice);
        item.ItemTypeId.Should().Be(itemParameters.ItemTypeId);
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
        var itemParameters = CatalogTestData.CreateItemParameters();
        var invalidParameters = itemParameters with { Name = invalidName };
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Create(invalidParameters));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmptyCode_ThrowsArgumentException(string invalidCode)
    {
        // Arrange
        var itemParameters = CatalogTestData.CreateItemParameters();
        var invalidParameters = itemParameters with { Code = invalidCode };
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Create(invalidParameters));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmptyUnitOfMeasurement_ThrowsArgumentException(string invalidUom)
    {
        // Arrange
        var itemParameters = CatalogTestData.CreateItemParameters();
        var invalidParameters = itemParameters with { UnitOfMeasurement = invalidUom };
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Create(invalidParameters));
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
        var itemParameters = new ItemParameters(
            CatalogTestData.CreateName(),
            CatalogTestData.CreateCode(),
            CatalogTestData.NextId(),
            CatalogTestData.CreateListPrice(),
            CatalogTestData.NextId(),
            CatalogTestData.CreateUnitOfMeasurement());
        // Act
        var item = Item.Create(itemParameters);
        // Assert
        item.Name.Should().Be(itemParameters.Name);
        item.Code.Should().Be(itemParameters.Code);
        item.Description.Should().Be(string.Empty);
        item.CategoryId.Should().Be(itemParameters.CategoryId);
        item.ListPrice.Should().Be(itemParameters.ListPrice);
        item.ItemTypeId.Should().Be(itemParameters.ItemTypeId);
        item.UnitOfMeasurement.Should().Be(itemParameters.UnitOfMeasurement);
        item.IsActive.Should().Be(true);
        item.Description.Should().Be(string.Empty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_InvalidCategoryId_ThrowsArgumentException(long invalidCategoryId)
    {
        // Arrange
        var itemParameters = CatalogTestData.CreateItemParameters();
        var invalidParameters = itemParameters with { CategoryId = invalidCategoryId };
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Create(invalidParameters));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_InvalidItemTypeId_ThrowsArgumentException(long invalidItemTypeId)
    {
        // Arrange
        var itemParameters = CatalogTestData.CreateItemParameters();
        var invalidParameters = itemParameters with { ItemTypeId = invalidItemTypeId };
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Create(invalidParameters));
    }

    [Fact]
    public void Create_NullMoney_ThrowsArgumentNullException()
    {
        // Arrange
        var itemParameters = CatalogTestData.CreateItemParameters();
        var invalidParameters = itemParameters with { ListPrice = null };
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Item.Create(invalidParameters));
    }

    [Fact]
    public void Reconstitute_ValidAllParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var id = CatalogTestData.NextId();
        var itemParameters = CatalogTestData.CreateItemParameters();
        // Act
        var item = Item.Reconstitute(id, itemParameters);

        // Assert
        item.Id.Should().Be(id);
        item.Name.Should().Be(itemParameters.Name);
        item.Code.Should().Be(itemParameters.Code);
        item.Description.Should().Be(itemParameters.Description);
        item.CategoryId.Should().Be(itemParameters.CategoryId);
        item.ListPrice.Should().Be(itemParameters.ListPrice);
        item.ItemTypeId.Should().Be(itemParameters.ItemTypeId);
        item.UnitOfMeasurement.Should().Be(itemParameters.UnitOfMeasurement);
        item.IsActive.Should().Be(itemParameters.IsActive);
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void Reconstitute_InvalidId_ThrowsArgumentException(long invalidId)
    {
        // Arrange
        var itemParameters = CatalogTestData.CreateItemParameters();
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
        var id = CatalogTestData.NextId();
        var itemParameters = CatalogTestData.CreateItemParameters();
        var invalidItemParameters = itemParameters with { Name = invalidName };
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Reconstitute(id, invalidItemParameters));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Reconstitute_NullOrEmptyCode_ThrowsArgumentException(string invalidCode)
    {
        // Arrange
        var id = CatalogTestData.NextId();
        var itemParameters = CatalogTestData.CreateItemParameters();
        var invalidItemParameters = itemParameters with { Code = invalidCode };
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Reconstitute(id, invalidItemParameters));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Reconstitute_NullOrEmptyUnitOfMeasurement_ThrowsArgumentException(string invalidUom)
    {
        // Arrange
        var id = CatalogTestData.NextId();
        var itemParameters = CatalogTestData.CreateItemParameters();
        var invalidItemParameters = itemParameters with { UnitOfMeasurement = invalidUom };
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Reconstitute(id, invalidItemParameters));
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
        var id = CatalogTestData.NextId();
        var itemParameters = new ItemParameters(
            CatalogTestData.CreateName(),
            CatalogTestData.CreateCode(),
            CatalogTestData.NextId(),
            CatalogTestData.CreateListPrice(),
            CatalogTestData.NextId(),
            CatalogTestData.CreateUnitOfMeasurement());
        // Act
        var item = Item.Reconstitute(id, itemParameters);
        // Assert
        item.Id.Should().Be(id);
        item.Name.Should().Be(itemParameters.Name);
        item.Code.Should().Be(itemParameters.Code);
        item.Description.Should().Be(string.Empty);
        item.CategoryId.Should().Be(itemParameters.CategoryId);
        item.ListPrice.Should().Be(itemParameters.ListPrice);
        item.ItemTypeId.Should().Be(itemParameters.ItemTypeId);
        item.UnitOfMeasurement.Should().Be(itemParameters.UnitOfMeasurement);
        item.IsActive.Should().Be(true);
        item.Description.Should().Be(string.Empty);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Reconstitute_InvalidCategoryId_ThrowsArgumentException(long invalidCategoryId)
    {
        // Arrange
        var id = CatalogTestData.NextId();
        var itemParameters = CatalogTestData.CreateItemParameters();
        var invalidItemParameters = itemParameters with { CategoryId = invalidCategoryId };
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Reconstitute(id, invalidItemParameters));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Reconstitute_InvalidItemTypeId_ThrowsArgumentException(long invalidItemTypeId)
    {
        // Arrange
        var id = CatalogTestData.NextId();
        var itemParameters = CatalogTestData.CreateItemParameters();
        var invalidItemParameters = itemParameters with { ItemTypeId = invalidItemTypeId };
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Reconstitute(id, invalidItemParameters));
    }

    [Fact]
    public void Reconstitute_NullMoney_ThrowsArgumentNullException()
    {
        // Arrange
        var id = CatalogTestData.NextId();
        var itemParameters = CatalogTestData.CreateItemParameters();
        var invalidItemParameters = itemParameters with { ListPrice = null };
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Item.Reconstitute(id, invalidItemParameters));
    }

    [Fact]
    public void UpdateItem_ValidParameters_UpdatesPropertiesCorrectly()
    {
        // Arrange
        var item = CatalogTestData.CreateItem();
        var updatedParameters = CatalogTestData.CreateItemParameters();
        // Act
        item.UpdateItem(updatedParameters);
        // Assert
        item.Name.Should().Be(updatedParameters.Name);
        item.Code.Should().Be(updatedParameters.Code);
        item.Description.Should().Be(updatedParameters.Description);
        item.CategoryId.Should().Be(updatedParameters.CategoryId);
        item.ListPrice.Should().Be(updatedParameters.ListPrice);
        item.ItemTypeId.Should().Be(updatedParameters.ItemTypeId);
        item.UnitOfMeasurement.Should().Be(updatedParameters.UnitOfMeasurement);
        item.IsActive.Should().Be(updatedParameters.IsActive);
    }

    [Fact]
    public void UpdateItem_NullItemParameters_ThrowsArgumentNullException()
    {
        // Arrange
        var item = CatalogTestData.CreateItem();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => item.UpdateItem(null));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void UpdateItem_InvalidCategoryId_ThrowsArgumentException(long invalidCategoryId)
    {
        // Arrange
        var item = CatalogTestData.CreateItem();
        var updatedParameters = CatalogTestData.CreateItemParameters();
        var invalidUpdateParameters = updatedParameters with { CategoryId = invalidCategoryId };
        // Act & Assert
        Assert.Throws<ArgumentException>(() => item.UpdateItem(invalidUpdateParameters));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void UpdateItem_InvalidItemTypeId_ThrowsArgumentException(long invalidItemTypeId)
    {
        // Arrange
        var item = CatalogTestData.CreateItem();
        var updatedParameters = CatalogTestData.CreateItemParameters();
        var invalidUpdateParameters = updatedParameters with { ItemTypeId = invalidItemTypeId };
        // Act & Assert
        Assert.Throws<ArgumentException>(() => item.UpdateItem(invalidUpdateParameters));
    }

    [Fact]
    public void UpdateItem_NullMoney_ThrowsArgumentNullException()
    {
        // Arrange
        var item = CatalogTestData.CreateItem();
        var updatedParameters = CatalogTestData.CreateItemParameters();
        var invalidUpdateParameters = updatedParameters with { ListPrice = null };
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => item.UpdateItem(invalidUpdateParameters));
    }

    [Fact]
    public void UpdateItem_SameValues_DoesNotChangeProperties()
    {
        // Arrange
        var initialParameters = CatalogTestData.CreateItemParameters();
        var item = Item.Create(initialParameters);
        // Act
        item.UpdateItem(initialParameters);
        // Assert
        item.Name.Should().Be(initialParameters.Name);
        item.Code.Should().Be(initialParameters.Code);
        item.Description.Should().Be(initialParameters.Description);
        item.CategoryId.Should().Be(initialParameters.CategoryId);
        item.ListPrice.Should().Be(initialParameters.ListPrice);
        item.ItemTypeId.Should().Be(initialParameters.ItemTypeId);
        item.UnitOfMeasurement.Should().Be(initialParameters.UnitOfMeasurement);
        item.IsActive.Should().Be(initialParameters.IsActive);
    }

    [Fact]
    public void UpdateItem_ActivateItem_SetsIsActiveToTrue()
    {
        // Arrange
        var initialParameters = CatalogTestData.CreateItemParameters();
        var deactivatedParameters = initialParameters with { IsActive = false };
        var item = Item.Create(deactivatedParameters);
        var updatedParameters = CatalogTestData.CreateItemParameters();
        var activatedParameters = updatedParameters with { IsActive = true };
        // Act
        item.UpdateItem(activatedParameters);
        // Assert
        item.IsActive.Should().BeTrue();
    }

    [Fact]
    public void UpdateItem_DeactivateItem_SetsIsActiveToFalse()
    {
        // Arrange
        var initialParameters = CatalogTestData.CreateItemParameters();
        var activedParameters = initialParameters with { IsActive = true };
        var item = Item.Create(activedParameters);
        var updatedParameters = CatalogTestData.CreateItemParameters();
        var deactivatedParameters = updatedParameters with { IsActive = false };
        // Act
        item.UpdateItem(deactivatedParameters);
        // Assert
        item.IsActive.Should().BeFalse();
    }

    [Fact]
    public void UpdateName_ValidName_UpdatesNameCorrectly()
    {
        // Arrange
        var item = CatalogTestData.CreateItem();
        var newName = $"Name_{Guid.NewGuid().ToString()[..8]}";
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
        var item = CatalogTestData.CreateItem();
        // Act & Assert
        Assert.Throws<ArgumentException>(() => item.UpdateName(invalidName));
    }

    [Fact]
    public void UpdateCode_ValidCode_UpdatesCodeCorrectly()
    {
        // Arrange
        var item = CatalogTestData.CreateItem();
        var newCode = $"Code_{Guid.NewGuid().ToString()[..8]}";
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
        var item = CatalogTestData.CreateItem();
        // Act & Assert
        Assert.Throws<ArgumentException>(() => item.UpdateCode(invalidCode));
    }

    [Fact]
    public void UpdateDescription_ValidDescription_UpdatesDescriptionCorrectly()
    {
        // Arrange
        var item = CatalogTestData.CreateItem();
        var newDescription = $"Description_{Guid.NewGuid().ToString()[..20]}";
        // Act
        item.UpdateDescription(newDescription);
        // Assert
        item.Description.Should().Be(newDescription);
    }

    [Fact]
    public void UpdateCategoryId_ValidCategoryId_UpdatesCategoryCorrectly()
    {
        // Arrange
        var item = CatalogTestData.CreateItem();
        var newCategoryId = CatalogTestData.NextId();
        // Act
        item.UpdateCategoryId(newCategoryId);
        // Assert
        item.CategoryId.Should().Be(newCategoryId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void UpdateCategory_InvalidCategoryId_ThrowsArgumentException(long invalidCategoryId)
    {
        // Arrange
        var item = CatalogTestData.CreateItem();
        // Act & Assert
        Assert.Throws<ArgumentException>(() => item.UpdateCategoryId(invalidCategoryId));
    }

    [Fact]
    public void UpdateListPrice_ValidMoney_UpdatesListPriceCorrectly()
    {
        // Arrange
        var item = CatalogTestData.CreateItem();
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
        var item = CatalogTestData.CreateItem();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => item.UpdateListPrice(null));
    }

    [Fact]
    public void UpdateItemTypeId_ValidItemTypeId_UpdatesItemTypeIdCorrectly()
    {
        // Arrange
        var item = CatalogTestData.CreateItem();
        var newItemType = CatalogTestData.NextId();
        // Act
        item.UpdateItemTypeId(newItemType);
        // Assert
        item.ItemTypeId.Should().Be(newItemType);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void UpdateItemTypeId_InvalidItemTypeId_ThrowsArgumentException(long invalidItemTypeId)
    {
        // Arrange
        var item = CatalogTestData.CreateItem();
        // Act & Assert
        Assert.Throws<ArgumentException>(() => item.UpdateItemTypeId(invalidItemTypeId));
    }

    [Fact]
    public void UpdateUnitOfMeasurement_ValidUom_UpdatesUnitOfMeasurementCorrectly()
    {
        // Arrange
        var item = CatalogTestData.CreateItem();
        var newUom = $"UoM_{Guid.NewGuid().ToString()[..5]}";
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
        var item = CatalogTestData.CreateItem();
        // Act & Assert
        Assert.Throws<ArgumentException>(() => item.UpdateUnitOfMeasurement(invalidUom));
    }

    [Fact]
    public void Activate_ItemIsInactive_SetsIsActiveToTrue()
    {
        // Arrange
        var itemParameters = CatalogTestData.CreateItemParameters();
        var deactivatedParameters = itemParameters with { IsActive = false };
        var item = Item.Create(deactivatedParameters);
        // Act
        item.Activate();
        // Assert
        item.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Deactivate_ItemIsActive_SetsIsActiveToFalse()
    {
        // Arrange
        var itemParameters = CatalogTestData.CreateItemParameters();
        var activatedParameters = itemParameters with { IsActive = true };
        var item = Item.Create(activatedParameters);
        // Act
        item.Deactivate();
        // Assert
        item.IsActive.Should().BeFalse();
    }
}
