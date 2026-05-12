using FluentAssertions;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.UnitTests.Catalog.Helpers;

namespace Ogma.Domain.UnitTests.Catalog.Tests;

public class CategoryTests
{
    [Fact]
    public void Create_ValidProperties_SetCorrectly()
    {
        // Arrange
        string name = "created category";
        long parentCategoryId = 3;
        string path = "1/2/3";

        // Act
        var category = Category.Create(name, parentCategoryId, path);

        // Assert
        category.Name.Should().Be(name);
        category.ParentCategoryId.Should().Be(parentCategoryId);
        category.Path.Should().Be(path);
    }

    [Fact]
    public void Create_OptionalParameters_SetCorrectly()
    {
        // Arrange
        string name = "created category";

        // Act
        var category = Category.Create(name);

        // Assert
        category.Name.Should().Be(name);
        category.ParentCategoryId.Should().Be(null);
        category.Path.Should().Be(null);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmptyName_ThrowsArgumentException(string invalidName)
    {
        Assert.Throws<ArgumentException>(() => Category.Create(invalidName));
    }

    [Fact]
    public void Create_ParentCategoryIdMismatchWithPath_ThrowsArgumentException()
    {
        // Arrange
        string name = "category";
        long parentCategoryId = 2;
        string path = "1/3/4";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Category.Create(name, parentCategoryId, path));
    }

    [Fact]
    public void Reconstitute_ValidProperties_SetCorrectly()
    {
        // Arrange
        long id = 4;
        string name = "reconstituted category";
        long parentCategoryId = 3;
        string path = "1/2/3";

        // Act
        var category = Category.Reconstitute(id, name, CatalogTestData.GetMetadata(), parentCategoryId, path);

        // Assert
        category.Id.Should().Be(id);
        category.Name.Should().Be(name);
        category.ParentCategoryId.Should().Be(parentCategoryId);
        category.Path.Should().Be(path);
    }

    [Fact]
    public void Reconstitute_OptionalParameters_SetCorrectly()
    {
        // Arrange
        long id = 1;
        string name = "created category";

        // Act
        var category = Category.Reconstitute(id, name, CatalogTestData.GetMetadata());

        // Assert
        category.Id.Should().Be(id);
        category.Name.Should().Be(name);
        category.ParentCategoryId.Should().Be(null);
        category.Path.Should().Be(null);
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void Reconstitute_InvalidId_ThrowsArgumentException(long invalidId)
    {
        Assert.Throws<ArgumentException>(() => Category.Reconstitute(invalidId, "category", CatalogTestData.GetMetadata()));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Reconstitute_NullOrEmptyName_ThrowsArgumentException(string invalidName)
    {
        Assert.Throws<ArgumentException>(() => Category.Reconstitute(1, invalidName, CatalogTestData.GetMetadata()));
    }

    [Theory]
    [InlineData("1/2/99")] // Path contains id
    [InlineData("2/3/5")] // ParentCategoryId mismatch
    [InlineData("2/3/4/5/99")] // Exceeds max depth
    [InlineData("2/2/99")] // Path with duplicate IDs
    public void Reconstitute_InvalidPath_ThrowsArgumentException(string path)
    {
        Assert.Throws<ArgumentException>(() => Category.Reconstitute(1, "category", CatalogTestData.GetMetadata(), 99, path));
    }

    [Fact]
    public void Update_ValidProperties_UpdatesPropertiesCorrectly()
    {
        // Arrange
        var category = Category.Create("initial name", 1);
        string newName = "updated name";
        long newParentCategoryId = 2;

        // Act
        category.Update(newName, newParentCategoryId);

        // Assert
        category.Name.Should().Be(newName);
        category.ParentCategoryId.Should().Be(newParentCategoryId);
    }

    [Fact]
    public void Update_ValidProperties_UpdatesMetadataCorrectly()
    {
        // Arrange
        var category = Category.Create("initial name", 1);
        var oldCategoryMetadata = new EntityMetadata(category.Metadata.CreatedAt, category.Metadata.UpdatedAt, category.Metadata.Version);
        string newName = "updated name";
        long newParentCategoryId = 2;

        // Act
        category.Update(newName, newParentCategoryId);

        // Assert
        category.Metadata.CreatedAt.Should().Be(oldCategoryMetadata.CreatedAt);
        category.Metadata.UpdatedAt.Should().BeAfter(oldCategoryMetadata.UpdatedAt);
        category.Metadata.Version.Should().Be(oldCategoryMetadata.Version + 1);
    }

    [Fact]
    public void Update_ParentNull_RemovesParent()
    {
        // Arrange
        var category = Category.Reconstitute(1, "initial name", CatalogTestData.GetMetadata(), 2);

        // Act
        category.Update(category.Name, null);

        // Assert
        category.ParentCategoryId.Should().Be(null);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_NullOrEmptyName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var category = Category.Create("initial name", 1);
        // Act & Assert
        Assert.Throws<ArgumentException>(() => category.Update(invalidName, 2));
    }

    [Fact]
    public void Update_ParentCategoryIdSameAsId_ThrowsInvalidOperationException()
    {
        // Arrange
        long categoryId = 1;
        string name = "initial name";
        long parentCategoryId = 2;
        var category = Category.Reconstitute(categoryId, name, CatalogTestData.GetMetadata(), parentCategoryId);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => category.Update("updated name", categoryId));
    }

    [Fact]
    public void UpdatePath_ValidString_SetsPath()
    {
        // Arrange
        var category = Category.Create("category", 3);
        string newPath = "1/2/3";

        // Act
        category.UpdatePath(newPath);

        // Assert
        category.Path.Should().Be(newPath);
    }

    [Fact]
    public void UpdatePath_Null_SetsPathToNull()
    {
        // Arrange
        var category = Category.Create("category", 4);
        string? newPath = null;

        // Act
        category.UpdatePath(newPath);

        // Assert
        category.Path.Should().BeNull();
    }

    [Theory]
    [InlineData("1/2/99")] // Path contains id
    [InlineData("2/3/5")] // ParentCategoryId mismatch
    [InlineData("2/3/4/5/99")] // Exceeds max depth
    [InlineData("2/2/99")] // Path with duplicate IDs
    public void UpdatePath_InvalidPath_ThrowsArgumentException(string path)
    {
        // Arrange
        var category = Category.Reconstitute(1, "category", CatalogTestData.GetMetadata(), 99);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => category.UpdatePath(path));
    }

    [Fact]
    public void AddSubCategory_ValidSubCategory_AddsSuccessfully()
    {
        // Arrange
        var parentCategory = Category.Reconstitute(1, "Parent Category", CatalogTestData.GetMetadata());
        var subCategory = Category.Reconstitute(2, "Sub Category", CatalogTestData.GetMetadata(), parentCategory.Id);

        // Act
        parentCategory.AddSubCategory(subCategory);

        // Assert
        parentCategory.SubCategories.Should().Contain(subCategory);
    }

    [Fact]
    public void AddSubCategory_NullSubCategory_ThrowsArgumentNullException()
    {
        // Arrange
        var parentCategory = Category.Reconstitute(1, "Parent Category", CatalogTestData.GetMetadata());

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => parentCategory.AddSubCategory(null!));
    }

    [Fact]
    public void AddSubCategory_MismatchedParentCategoryId_ThrowsInvalidOperationException()
    {
        // Arrange
        var parentCategory = Category.Reconstitute(1, "Parent Category", CatalogTestData.GetMetadata());
        var subCategory = Category.Reconstitute(2, "Sub Category", CatalogTestData.GetMetadata(), 999); // Mismatched ParentCategoryId

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => parentCategory.AddSubCategory(subCategory));
    }

    [Fact]
    public void AddSubCategory_MaxDepthReached_ThrowsInvalidOperationException()
    {
        // Arrange
        var parentCategory = Category.Reconstitute(1, "Parent", CatalogTestData.GetMetadata(), null, "2/3/4/5"); // Depth = 4, RemainingDepth = 0

        var subCategory = Category.Create("Sub", 1);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => parentCategory.AddSubCategory(subCategory));
    }

    [Fact]
    public void AddSubCategory_MultipleTimes_ThrowsInvalidOperationException()
    {
        // Arrange
        var parentCategory = Category.Reconstitute(1, "Parent Category", CatalogTestData.GetMetadata());
        var subCategory = Category.Reconstitute(2, "Sub Category", CatalogTestData.GetMetadata(), parentCategory.Id);

        // Act
        parentCategory.AddSubCategory(subCategory);

        // Assert
        Assert.Throws<InvalidOperationException>(() => parentCategory.AddSubCategory(subCategory));
    }

    [Fact]
    public void AddSubCategories_ValidCollection_AddsAll()
    {
        // Arrange
        var parentCategory = Category.Reconstitute(1, "Parent Category", CatalogTestData.GetMetadata());
        var subCategories = new List<Category>
        {
            Category.Reconstitute(2, "Sub Category 1", CatalogTestData.GetMetadata(), parentCategory.Id),
            Category.Reconstitute(3, "Sub Category 2", CatalogTestData.GetMetadata(), parentCategory.Id)
        };

        // Act
        parentCategory.AddSubCategories(subCategories);

        // Assert
        parentCategory.SubCategories.Should().Contain(subCategories);
    }

    [Fact]
    public void AddSubCategories_NullCollection_ThrowsArgumentNullException()
    {
        // Arrange
        var parentCategory = Category.Reconstitute(1, "Parent Category", CatalogTestData.GetMetadata());
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => parentCategory.AddSubCategories(null!));
    }

    [Fact]
    public void AddSubCategories_OneInvalidSubCategory_ThrowsInvalidOperationException()
    {
        // Arrange
        var parentCategory = Category.Reconstitute(1, "Parent Category", CatalogTestData.GetMetadata());
        var subCategories = new List<Category>
        {
            Category.Reconstitute(2, "Sub Category 1", CatalogTestData.GetMetadata(), parentCategory.Id),
            Category.Reconstitute(3, "Sub Category 2", CatalogTestData.GetMetadata(), 999)
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => parentCategory.AddSubCategories(subCategories));
    }

    [Fact]
    public void AddSubCategories_MaxDepthReached_ThrowsInvalidOperationException()
    {
        // Arrange
        var parentCategory = Category.Reconstitute(1, "Parent", CatalogTestData.GetMetadata(), null, "2/3/4/5"); // Depth = 4, RemainingDepth = 0

        var subCategories = new List<Category>
        {
            Category.Reconstitute(99, "Sub 99", CatalogTestData.GetMetadata(), 1),
            Category.Reconstitute(100, "Sub 100", CatalogTestData.GetMetadata(), 1),
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => parentCategory.AddSubCategories(subCategories));
    }

    [Fact]
    public void GetFullPath_NoExistingPath_ReturnsIdAsString()
    {
        // Arrange
        var category = Category.Reconstitute(42, "Category Without Path", CatalogTestData.GetMetadata());

        // Act
        var fullPath = category.GetFullPath();

        // Assert
        fullPath.Should().Be("42");
    }

    [Fact]
    public void GetFullPath_ExistingPath_ReturnsConcatenatedPath()
    {
        // Arrange
        var category = Category.Reconstitute(42, "Category With Path", CatalogTestData.GetMetadata(), null, "1/2/3");

        // Act
        var fullPath = category.GetFullPath();

        // Assert
        fullPath.Should().Be("1/2/3/42");
    }

    [Fact]
    public void SubCategories_InitiallyEmpty_ReturnsEmptyCollection()
    {
        // Arrange
        var category = Category.Create("Category");

        // Act
        var subCategories = category.SubCategories;

        // Assert
        subCategories.Should().BeEmpty();
    }

    [Fact]
    public void Depth_RootCategory_IsZero()
    {
        // Arrange
        var category = Category.Create("Root Category");

        // Act
        var depth = category.Depth;

        // Assert
        depth.Should().Be(0);
    }

    [Fact]
    public void Depth_EmptyPath_IsZero()
    {
        // Arrange
        var category = Category.Create("Category", null, "");

        // Act
        var depth = category.Depth;

        // Assert
        depth.Should().Be(0);
    }

    [Fact]
    public void Depth_NonRootCategory_ComputesCorrectDepth()
    {
        // Arrange
        var category = Category.Create("Category", 4, "1/2/3/4");

        // Act
        var depth = category.Depth;

        // Assert
        depth.Should().Be(4);
    }

    [Fact]
    public void RemainingDepth_MaxDepthMinusCurrentDepth_ComputesCorrectly()
    {
        // Arrange
        var category = Category.Create("Category", 3, "1/2/3");

        // Act
        var remainingDepth = category.RemainingDepth;

        // Assert
        remainingDepth.Should().Be(1);
    }

    [Fact]
    public void RemainingDepth_AtMaxDepth_IsZero()
    {
        // Arrange
        var category = Category.Create("Category", 4, "1/2/3/4");

        // Act
        var remainingDepth = category.RemainingDepth;

        // Assert
        remainingDepth.Should().Be(0);
    }

    [Fact]
    public void IsRootCategory_WhenParentNull_ReturnsTrue()
    {
        // Arrange
        var category = Category.Create("Root Category");

        // Act
        var isRoot = category.IsRootCategory();

        // Assert
        isRoot.Should().BeTrue();
    }

    [Fact]
    public void IsRootCategory_WhenParentNotNull_ReturnsFalse()
    {
        // Arrange
        var category = Category.Create("Child Category", 1);

        // Act
        var isRoot = category.IsRootCategory();

        // Assert
        isRoot.Should().BeFalse();
    }

    [Fact]
    public void SubCategories_ReturnsReadOnlyWrapper()
    {
        // Arrange
        var parent = Category.Create("Parent");
        var child = Category.Create("Child", parent.Id, parent.GetFullPath());
        parent.AddSubCategory(child);

        // Act
        var subCategories = parent.SubCategories;

        // Assert
        // 1. Verify it’s an IReadOnlyCollection<Category>
        subCategories.Should().BeAssignableTo<IReadOnlyCollection<Category>>();

        // 2. Verify it contains the expected subcategory
        subCategories.Should().ContainSingle().Which.Should().Be(child);

        // 3. Verify it cannot be cast to List<Category> (to prevent modification)
        subCategories.Should().NotBeOfType<List<Category>>();

        // 4. Attempting to cast and modify should fail (optional, for explicitness)
        Action modifyAction = () =>
        {
            if (subCategories is List<Category> mutableList)
            {
                mutableList.Add(Category.Create("Invalid", null, null));
            }
        };
        modifyAction.Should().NotThrow(); // No exception, but cast fails, so no modification occurs

        // 5. Verify the collection is still intact
        subCategories.Should().ContainSingle().Which.Should().Be(child);
    }

    [Fact]
    public void SubCategories_ModifyingInternalList_ReflectsChangesInSubsequentCalls()
    {
        // Arrange
        var parent = Category.Create("Parent");
        var firstChild = Category.Create("FirstChild", parent.Id, null);
        parent.AddSubCategory(firstChild);

        // Act
        var initialSubCategories = parent.SubCategories.ToList(); // Create a copy
        var secondChild = Category.Create("SecondChild", parent.Id, null);
        parent.AddSubCategory(secondChild); // Modifies _subCategories

        // Assert
        initialSubCategories.Should().ContainSingle().Which.Should().Be(firstChild);
        parent.SubCategories.Should().HaveCount(2)
            .And.Contain(firstChild)
            .And.Contain(secondChild);
    }

    [Fact]
    public void GetAllDescendants_EmptyCategory_ReturnsEmpty()
    {
        // Arrange
        var parent = Category.Create("Parent");

        // Act 
        var subCategories = parent.GetAllDescendants();

        // Assert
        subCategories.Should().HaveCount(0);
    }

    [Fact]
    public void GetAllDescendants_SingleLevel_ReturnsDirectChildren()
    {
        // Arrange
        var parent = Category.Create("Parent");
        var child1 = Category.Create("Child1", parent.Id, null);
        var child2 = Category.Create("Child2", parent.Id, null);
        parent.AddSubCategory(child1);
        parent.AddSubCategory(child2);

        // Act 
        var subCategories = parent.GetAllDescendants().ToList();

        // Assert 
        subCategories.Should().HaveCount(2)
        .And.Contain(child1)
        .And.Contain(child2)
        .And.NotContain(parent)
        .And.BeEquivalentTo(parent.SubCategories);
    }

    [Fact]
    public void GetAllDescendants_MultiLevel_ReturnsAllRecursive()
    {
        // Arrange
        var parent = Category.Create("Parent");
        var child1 = Category.Create("Child1", parent.Id, null);
        var child2 = Category.Create("Child2", parent.Id, null);
        var grandchild11 = Category.Create("Grandchild1", child1.Id, null);
        var grandchild21 = Category.Create("Grandchild2", child2.Id, null);
        var grandchild22 = Category.Create("Grandchild3", child2.Id, null);

        parent.AddSubCategory(child1);
        parent.AddSubCategory(child2);
        child1.AddSubCategory(grandchild11);
        child2.AddSubCategory(grandchild21);
        child2.AddSubCategory(grandchild22);

        // Verify hierarchy setup
        parent.SubCategories.Should().HaveCount(2)
            .And.Contain(child1)
            .And.Contain(child2);
        child1.SubCategories.Should().ContainSingle()
            .And.Contain(grandchild11);
        child2.SubCategories.Should().HaveCount(2)
            .And.Contain(grandchild21)
            .And.Contain(grandchild22);

        // Act
        var descendants = parent.GetAllDescendants().ToList();

        // Assert
        descendants.Should().HaveCount(5)
            .And.Contain(child1)
            .And.Contain(child2)
            .And.Contain(grandchild11)
            .And.Contain(grandchild21)
            .And.Contain(grandchild22)
            .And.NotContain(parent)
            .And.ContainInOrder(child1, grandchild11, child2, grandchild21, grandchild22);
    }

    [Fact]
    public void Create_NameWithSpecialCharacters_Accepted()
    {
        // Arrange
        string name = "Category! @# $%^ &*()_+";

        // Act
        var category = Category.Create(name);

        // Assert
        category.Name.Should().Be(name);
    }

    [Fact]
    public void Depth_PathWithTrailingSlash_StillCountsCorrectly()
    {
        // Arrange
        var category = Category.Create("category", 3, "1/2/3///");

        // Act & Assert
        category.Depth.Should().Be(3);
    }

}