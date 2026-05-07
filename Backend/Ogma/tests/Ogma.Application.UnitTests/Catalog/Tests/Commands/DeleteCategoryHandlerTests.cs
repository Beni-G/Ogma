using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Commands;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;
using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Application.UnitTests.Catalog.Tests.Commands;
public class DeleteCategoryHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryStub;
    private readonly DeleteCategoryHandler _handler;
    private readonly EntityMetadata _metadata;

    public DeleteCategoryHandlerTests()
    {
        _categoryRepositoryStub = new Mock<ICategoryRepository>();
        _handler = new DeleteCategoryHandler(_categoryRepositoryStub.Object);
        _metadata = new EntityMetadata(DateTime.UtcNow, DateTime.UtcNow, 1);
    }

    [Fact]
    public async Task Handle_ExistingCategory_DeletesCategory()
    {
        // Arrange
        long id = 1;
        var existingCategory = Category.Reconstitute(id, "CategoryToDelete", _metadata, null, "");
        var command = new DeleteCategoryCommand(id);
        _categoryRepositoryStub.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(existingCategory);
        _categoryRepositoryStub.Setup(repo => repo.DeleteAsync(existingCategory))
            .Returns(Task.CompletedTask);
        // Act
        await _handler.Handle(command, CancellationToken.None);
        // Assert
        _categoryRepositoryStub.Verify(r => r.GetByIdAsync(id), Times.Once);
        _categoryRepositoryStub.Verify(r => r.DeleteAsync(existingCategory), Times.Once);
    }

    [Fact]
    public async Task Handle_DeleteAsyncThrows_PropagatesException()
    {
        // Arrange
        var category = Category.Reconstitute(1L, "Category", _metadata, null, "");
        var command = new DeleteCategoryCommand(category.Id);

        _categoryRepositoryStub.Setup(r => r.GetByIdAsync(category.Id))
            .ReturnsAsync(category);
        _categoryRepositoryStub.Setup(r => r.DeleteAsync(category))
            .ThrowsAsync(new InvalidOperationException("Delete failed"));

        var handler = new DeleteCategoryHandler(_categoryRepositoryStub.Object);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Delete failed*");
    }



}
