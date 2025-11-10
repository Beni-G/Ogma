using FluentAssertions;
using Moq;
using Ogma.Application.Catalog.Commands;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.UnitTests.Commands;
public class DeleteCategoryHandlerTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryStub;
    private readonly DeleteCategoryHandler _handler;

    public DeleteCategoryHandlerTests()
    {
        _categoryRepositoryStub = new Mock<ICategoryRepository>();
        _handler = new DeleteCategoryHandler(_categoryRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ExistingCategory_DeletesCategory()
    {
        // Arrange
        long id = 1;
        var existingCategory = Category.Reconstitute(id, "CategoryToDelete", null, "");
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
        var category = Category.Reconstitute(1L, "Category", null, "");
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
