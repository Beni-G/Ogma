using FluentAssertions;
using Ogma.Application.Catalog.Services;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Services;

namespace Ogma.Application.UnitTests.Catalog.Tests.Services;
public class CategoryDomainServiceTests
{
    private readonly ICategoryDomainService _categoryDomainService = new CategoryDomainService();

    [Fact]
    public void ComputePath_ValidParent_ReturnsCorrectPath()
    {
        // Arrange
        var parentCategory = Category.Reconstitute(1L, "Parent", null, "");
        var category = Category.Reconstitute(2L, "Category", parentCategory.Id, "");
        // Act
        var result = _categoryDomainService.ComputePath(category, parentCategory);
        // Assert
        result.Should().Be(parentCategory.GetFullPath());
    }

    [Fact]
    public void ComputePath_ParentIsNul_ReturnsEmptyStringl()
    {
        // Arrange
        var category = Category.Reconstitute(1L, "Category", null, "");
        // Act
        var result = _categoryDomainService.ComputePath(category, null);
        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ComputePath_ParentDepthExceedsMaxDepth_ThrowsInvalidOperationException()
    {
        // Arrange
        var parentCategory = Category.Reconstitute(1L, "Parent", null, "");
        for (int i = 0; i < Category.MaxDepth; i++)
        {
            parentCategory = Category.Reconstitute(i + 2L, $"Subcategory{i + 1}", parentCategory.Id, parentCategory.GetFullPath());
        }
        var category = Category.Reconstitute(100L, "Category", parentCategory.Id, "");
        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _categoryDomainService.ComputePath(category, parentCategory));
    }

    [Fact]
    public void ComputePath_SettingDescendantAsParent_ThrowsInvalidOperationException()
    {
        // Arrange
        var parentCategory = Category.Reconstitute(1L, "Parent", null, "");
        var childCategory = Category.Reconstitute(2L, "Child", parentCategory.Id, parentCategory.GetFullPath());
        var grandChildCategory = Category.Reconstitute(3L, "GrandChild", childCategory.Id, childCategory.GetFullPath());
        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _categoryDomainService.ComputePath(parentCategory, grandChildCategory));
    }
}
