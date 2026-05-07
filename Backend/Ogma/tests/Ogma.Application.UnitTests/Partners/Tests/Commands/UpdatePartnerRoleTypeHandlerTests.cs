using FluentAssertions;
using Moq;
using Ogma.Application.Partners.Commands;
using Ogma.Application.UnitTests.Partners.Helpers;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.Partners.Repositories;

namespace Ogma.Application.UnitTests.Partners.Tests.Commands;

public class UpdatePartnerRoleTypeHandlerTests
{
    private readonly Mock<IPartnerRoleTypeRepository> _partnerRoleTypeRepositoryStub;
    private readonly UpdatePartnerRoleTypeHandler _handler;

    public UpdatePartnerRoleTypeHandlerTests()
    {
        _partnerRoleTypeRepositoryStub = new Mock<IPartnerRoleTypeRepository>();
        _handler = new UpdatePartnerRoleTypeHandler(_partnerRoleTypeRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ValidPartnerRoleType_ReturnsUpdatedPartnerRoleType()
    {
        // Arrange
        long id = 1;
        var existingPartnerRoleType = PartnerRoleType.Reconstitute(id, "ExistingCode", "ExistingName", PartnersTestData.GetMetadata(), "#FFFFFF");
        var command = new UpdatePartnerRoleTypeCommand(id, "UpdatedCode", "UpdatedName", "#000000");
        _partnerRoleTypeRepositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<PartnerRoleType>()))
            .ReturnsAsync((PartnerRoleType partnerRoleType) => true);
        _partnerRoleTypeRepositoryStub.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(existingPartnerRoleType);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.Code.Should().Be(command.Code);
        result.Name.Should().Be(command.Name);
        result.Color.Should().Be(command.Color);
        _partnerRoleTypeRepositoryStub.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once);
        _partnerRoleTypeRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<PartnerRoleType>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingPartnerRoleType_ReturnsKeyNotFoundException()
    {
        // Arrange
        long id = 1;
        var command = new UpdatePartnerRoleTypeCommand(id, "UpdatedCode", "UpdatedName", "#000000");
        _partnerRoleTypeRepositoryStub.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((PartnerRoleType?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString())); 
        _partnerRoleTypeRepositoryStub.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once);
        _partnerRoleTypeRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<PartnerRoleType>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UpdateFails_ReturnsInvalidOperationException()
    {
        // Arrange
        long id = 1;
        var existingPartnerRoleType = PartnerRoleType.Reconstitute(id, "ExistingCode", "ExistingName", PartnersTestData.GetMetadata(), "#FFFFFF");
        var command = new UpdatePartnerRoleTypeCommand(id, "UpdatedCode", "UpdatedName", "#000000");
        _partnerRoleTypeRepositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<PartnerRoleType>()))
            .ReturnsAsync(false);
        _partnerRoleTypeRepositoryStub.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(existingPartnerRoleType);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex.Message.Contains(id.ToString())); ;
        _partnerRoleTypeRepositoryStub.Verify(r => r.GetByIdAsync(It.IsAny<long>()), Times.Once);
        _partnerRoleTypeRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<PartnerRoleType>()), Times.Once);
    }
}
