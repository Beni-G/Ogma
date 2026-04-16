using FluentAssertions;
using Moq;
using Ogma.Application.Partners.Commands;
using Ogma.Application.UnitTests.Partners.Helpers;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.Partners.Repositories;

namespace Ogma.Application.UnitTests.Partners.Tests.Commands;

public class DeletePartnerHandlerTests
{
    private readonly Mock<IPartnerRepository> _partnerRepositoryStub;
    private readonly DeletePartnerHandler _handler;

    public DeletePartnerHandlerTests()
    {
        _partnerRepositoryStub = new Mock<IPartnerRepository>();
        _handler = new DeletePartnerHandler(_partnerRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ExistingPartner_DeletesPartner()
    {
        // Arrange
        var partner = new PartnerBuilder()
            .AsLegalEntity()
            .Build();
        var command = new DeletePartnerCommand(partner.Id);
        _partnerRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(partner);
        // Act
        await _handler.Handle(command, CancellationToken.None);
        // Aassert
        _partnerRepositoryStub.Verify(r => r.GetByIdAsync(partner.Id), Times.Once);
        _partnerRepositoryStub.Verify(r => r.DeleteAsync(partner), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        long id = 1L;
        var command = new DeletePartnerCommand(id);
        _partnerRepositoryStub.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Partner?)null);
        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _partnerRepositoryStub.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        _partnerRepositoryStub.Verify(repo => repo.DeleteAsync(It.IsAny<Partner>()), Times.Never);
    }
}
