using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Commands;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Domain.Catalog.Services;

namespace Ogma.Application.UnitTests.Commands;
public class CreateCategoryHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryStub;
    private readonly Mock<ICategoryDomainService> _categoryDomainServiceStub;
    private readonly CreateCategoryHandler _handler;

    public CreateCategoryHandlerTests()
    {
        _categoryRepositoryStub = new Mock<ICategoryRepository>();
        _categoryDomainServiceStub = new Mock<ICategoryDomainService>();
        _handler = new CreateCategoryHandler(_categoryRepositoryStub.Object, _categoryDomainServiceStub.Object);
    }

    [Fact]
    public async Task Handle_ValidRootCategory_ReturnsCreatedCategory()
    {
        // Arrange
        var command = new CreateCategoryCommand("NewCategory", null);
        long nextId = 1;
        _categoryDomainServiceStub.Setup(svc => svc.ComputePath(It.IsAny<Category>(), null))
            .Returns("");
        _categoryRepositoryStub.Setup(repo => repo.AddAsync(It.IsAny<Category>()))
            .ReturnsAsync((Category category) =>
            {
                return Category.Reconstitute(nextId, category.Name, category.ParentCategoryId, category.Path);
            });
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(nextId);
        result.Name.Should().Be(command.Name);
        result.ParentCategoryId.Should().BeNull();
        result.Path.Should().Be("");
        _categoryRepositoryStub.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidChildCategory_ReturnsCreatedCategory()
    {
        // Arrange
        var parentCategory = Category.Reconstitute(1, "ParentCategory", null, "");
        var command = new CreateCategoryCommand("ChildCategory", parentCategory.Id);
        long nextId = 2;
        _categoryRepositoryStub.Setup(repo => repo.GetByIdAsync(parentCategory.Id))
            .ReturnsAsync(parentCategory);
        _categoryDomainServiceStub.Setup(svc => svc.ComputePath(It.IsAny<Category>(), parentCategory))
            .Returns("1");
        _categoryRepositoryStub.Setup(repo => repo.AddAsync(It.IsAny<Category>()))
            .ReturnsAsync((Category category) =>
            {
                return Category.Reconstitute(nextId, category.Name, category.ParentCategoryId, category.Path);
            });
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(nextId);
        result.Name.Should().Be(command.Name);
        result.ParentCategoryId.Should().Be(parentCategory.Id);
        result.Path.Should().Be("1");
        _categoryRepositoryStub.Verify(r => r.GetByIdAsync(parentCategory.Id), Times.Once);
        _categoryRepositoryStub.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingParentCategory_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new CreateCategoryCommand("ChildCategory", 999);
        _categoryRepositoryStub.Setup(repo => repo.GetByIdAsync(command.ParentCategoryId.Value))
            .ReturnsAsync((Category?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.ParentCategoryId.Value.ToString()));
        _categoryRepositoryStub.Verify(r => r.GetByIdAsync(command.ParentCategoryId.Value), Times.Once);
        _categoryRepositoryStub.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Never);
    }
}
