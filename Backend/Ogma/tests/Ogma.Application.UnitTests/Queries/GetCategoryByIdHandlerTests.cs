using Moq;
using Ogma.Application.Catalog.Queries;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Domain.Catalog.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogma.Application.UnitTests.Queries;
public class GetCategoryByIdHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryStub ;
    private readonly GetCategoryByIdHandler _handler;

    public GetCategoryByIdHandlerTests()
    {
        _categoryRepositoryStub = new Mock<ICategoryRepository>();
        _handler = new GetCategoryByIdHandler(_categoryRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_CategoryExists_ReturnsCategoryWithDescendantsDto()
    {
        // Arrange
        var category = Category.Reconstitute(1L, "Category1", null, "");
        var subCategory = Category.Reconstitute(2L, "SubCategory1", 1L, "1");
        category.AddSubCategory(subCategory);
        _categoryRepositoryStub.Setup(repo => repo.GetByIdAsync(1L))
            .ReturnsAsync(category);
        // Act
        var result = await _handler.Handle(new GetCategoryByIdQuery(1L), CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1L);
        result.Name.Should().Be("Category1");
        result.SubCategories.Should().HaveCount(1);
        var subCategoryDto = result.SubCategories.First();
        subCategoryDto.Id.Should().Be(2L);
        subCategoryDto.Name.Should().Be("SubCategory1");
        _categoryRepositoryStub.Verify(r => r.GetByIdAsync(1L), Times.Once);
    }

    [Fact]
    public async Task Handle_CategoryDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        long id = 1L;
        _categoryRepositoryStub.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync((Category?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(new GetCategoryByIdQuery(id), CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _categoryRepositoryStub.Verify(r => r.GetByIdAsync(1L), Times.Once);
    }
}
