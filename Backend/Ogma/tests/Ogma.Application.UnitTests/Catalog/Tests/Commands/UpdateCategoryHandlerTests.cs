using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Commands;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Domain.Catalog.Services;
using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Application.UnitTests.Catalog.Tests.Commands;
public class UpdateCategoryHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryStub;
    private readonly Mock<ICategoryDomainService> _categoryDomainServiceStub;
    private readonly UpdateCategoryHandler _handler;
    private readonly EntityMetadata _metadata;

    public UpdateCategoryHandlerTests()
    {
        _categoryRepositoryStub = new Mock<ICategoryRepository>();
        _categoryDomainServiceStub = new Mock<ICategoryDomainService>();
        _handler = new UpdateCategoryHandler(_categoryRepositoryStub.Object, _categoryDomainServiceStub.Object);
        _metadata = new EntityMetadata(DateTime.UtcNow, DateTime.UtcNow, 1);
    }

    [Fact]
    public async Task Handle_ValidRootCategory_ReturnsUpdatedCategory()
    {
        // Arrange
        long id = 1;
        var existingCategory = Category.Reconstitute(id, "ExistingName", _metadata, null, "");
        var command = new UpdateCategoryCommand(id, "UpdatedName", null);
        _categoryRepositoryStub.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(existingCategory);
        _categoryRepositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<Category>()))
            .ReturnsAsync(true);
        _categoryDomainServiceStub.Setup(svc => svc.ComputePath(It.IsAny<Category>(), null))
            .Returns("");
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Name.Should().Be(command.Name);
        result.ParentCategoryId.Should().BeNull();
        result.Path.Should().Be("");
        _categoryRepositoryStub.Verify(r => r.GetByIdAsync(id), Times.Once);
        _categoryRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Once);
    }

    [Fact]
    public async Task Handle_UpdateChildCategory_ReturnsUpdatedCategory()
    {
        // Arrange
        var parentCategory = Category.Reconstitute(1L, "ParentCategory", _metadata, null, "");
        var childCategory = Category.Reconstitute(2L, "ExistingChildCategory", _metadata, 1, "1");
        var newParentCategory = Category.Reconstitute(3L, "NewParentCategory", _metadata, null, "");

        var categories = new Dictionary<long, Category>
            {
                { parentCategory.Id, parentCategory },
                { childCategory.Id, childCategory },
                { newParentCategory.Id, newParentCategory }
            };

        var command = new UpdateCategoryCommand(
            childCategory.Id,
            "UpdateExistingChildCategory",
            newParentCategory.Id
        );

        _categoryRepositoryStub
            .Setup(repo => repo.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((long id) =>
            {
                categories.TryGetValue(id, out var category);
                return category;
            });

        _categoryRepositoryStub
            .Setup(repo => repo.UpdateAsync(It.IsAny<Category>()))
            .ReturnsAsync(true);

        _categoryDomainServiceStub
            .Setup(svc => svc.ComputePath(It.IsAny<Category>(), It.IsAny<Category?>()))
            .Returns("3");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(childCategory.Id);
        result.Name.Should().Be(command.Name);
        result.ParentCategoryId.Should().Be(newParentCategory.Id);
        result.Path.Should().Be("3");

        _categoryRepositoryStub.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Exactly(2));
        _categoryRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Once);
        _categoryDomainServiceStub.Verify(svc => svc.ComputePath(It.IsAny<Category>(), It.IsAny<Category?>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingCategory_ReturnsKeyNotFoundException()
    {
        // Arrange
        long id = 1;
        _categoryRepositoryStub.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Category?)null);
        var command = new UpdateCategoryCommand(id, "UpdatedName", null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString())); ;
        _categoryRepositoryStub.Verify(r => r.GetByIdAsync(id), Times.Once);
        _categoryRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
        _categoryDomainServiceStub.Verify(svc => svc.ComputePath(It.IsAny<Category>(), It.IsAny<Category?>()), Times.Never);
    }

    [Fact]
    public async Task Handle_NonExistingParent_ReturnsKeyNotFoundException()
    {
        // Arrange
        long id = 1;
        var existingCategory = Category.Reconstitute(id, "ExistingName", _metadata, null, "");
        _categoryRepositoryStub.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(existingCategory);
        var command = new UpdateCategoryCommand(id, "UpdatedName", 2);
        _categoryRepositoryStub.Setup(repo => repo.GetByIdAsync((long)command.ParentCategoryId)).ReturnsAsync((Category?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.ParentCategoryId.Value.ToString()));
        _categoryRepositoryStub.Verify(r => r.GetByIdAsync(id), Times.Once);
        _categoryRepositoryStub.Verify(r => r.GetByIdAsync((long)command.ParentCategoryId), Times.Once);
        _categoryRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
        _categoryDomainServiceStub.Verify(svc => svc.ComputePath(It.IsAny<Category>(), It.IsAny<Category?>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UpdateFails_ReturnsInvalidOperationException()
    {
        // Arrange
        long id = 1;
        var existingCategory = Category.Reconstitute(id, "ExistingName", _metadata, null, "");
        var command = new UpdateCategoryCommand(id, "UpdatedName", null);
        _categoryRepositoryStub.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(existingCategory);
        _categoryRepositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<Category>()))
            .ReturnsAsync(false);
        _categoryDomainServiceStub.Setup(svc => svc.ComputePath(It.IsAny<Category>(), null))
            .Returns("");
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _categoryRepositoryStub.Verify(r => r.GetByIdAsync(id), Times.Once);
        _categoryRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Once);
        _categoryDomainServiceStub.Verify(svc => svc.ComputePath(It.IsAny<Category>(), null), Times.Once);
    }
}
