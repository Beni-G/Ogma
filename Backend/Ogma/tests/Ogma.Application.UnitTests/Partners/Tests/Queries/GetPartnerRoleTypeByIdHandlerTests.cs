using FluentAssertions;
using Moq;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Ports;
using Ogma.Application.Partners.Queries;
using Ogma.Application.UnitTests.Partners.Helpers;

namespace Ogma.Application.UnitTests.Partners.Tests.Queries;

public class GetPartnerRoleTypeByIdHandlerTests
{
    private readonly Mock<IPartnerRoleTypeReader> _partnerRoleTypeReaderStub;
    private readonly GetPartnerRoleTypeByIdHandler _handler;

    public GetPartnerRoleTypeByIdHandlerTests()
    {
        _partnerRoleTypeReaderStub = new Mock<IPartnerRoleTypeReader>();
        _handler = new GetPartnerRoleTypeByIdHandler(_partnerRoleTypeReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsPartnerRoleType()
    {
        // Arrange
        var partnerRoleType = PartnersTestData.CreatePartnerRoleTypeDto();
        _partnerRoleTypeReaderStub.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(partnerRoleType);
        // Act
        var result = await _handler.Handle(new GetPartnerRoleTypeByIdQuery(partnerRoleType.Id), CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(partnerRoleType);
        _partnerRoleTypeReaderStub.Verify(repo => repo.GetByIdAsync(partnerRoleType.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingId_ReturnsKeyNotFoundException()
    {
        // Arrange
        long id = 1L;
        _partnerRoleTypeReaderStub.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync((PartnerRoleTypeDto?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(new GetPartnerRoleTypeByIdQuery(id), CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _partnerRoleTypeReaderStub.Verify(repo => repo.GetByIdAsync(id), Times.Once);
    }
}
