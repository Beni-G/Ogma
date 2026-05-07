using FluentAssertions;
using Moq;
using Ogma.Application.Partners.Commands;
using Ogma.Application.UnitTests.Partners.Helpers;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.Partners.Repositories;

namespace Ogma.Application.UnitTests.Partners.Tests.Commands;

public class DeletePartnerRoleTypeHandlerTests
{
    private readonly Mock<IPartnerRoleTypeRepository> _partnerRoleTypeRepositoryMock;
    private readonly DeletePartnerRoleTypeHandler _handler;

    public DeletePartnerRoleTypeHandlerTests()
    {
        _partnerRoleTypeRepositoryMock = new Mock<IPartnerRoleTypeRepository>();
        _handler = new DeletePartnerRoleTypeHandler(_partnerRoleTypeRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingPartnerRoleType_DeletesPartnerRoleType()
    {
        // Arrange
        long id = 1;
        var existingPartnerRoleType = PartnerRoleType.Reconstitute(id, "CODE", "Name", PartnersTestData.GetMetadata(), "#FFFFFF");
        var command = new DeletePartnerRoleTypeCommand(id);
        _partnerRoleTypeRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(existingPartnerRoleType);
        _partnerRoleTypeRepositoryMock.Setup(repo => repo.DeleteAsync(existingPartnerRoleType))
            .Returns(Task.CompletedTask);
        // Act
        await _handler.Handle(command, CancellationToken.None);
        // Assert
        _partnerRoleTypeRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        _partnerRoleTypeRepositoryMock.Verify(repo => repo.DeleteAsync(existingPartnerRoleType), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        long id = 1;
        var command = new DeletePartnerRoleTypeCommand(id);
        _partnerRoleTypeRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync((PartnerRoleType?)null);
        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _partnerRoleTypeRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        _partnerRoleTypeRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<PartnerRoleType>()), Times.Never);
    }
}
