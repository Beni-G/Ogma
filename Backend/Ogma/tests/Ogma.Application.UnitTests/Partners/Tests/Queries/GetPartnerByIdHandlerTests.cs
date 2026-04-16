using FluentAssertions;
using Moq;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Ports;
using Ogma.Application.Partners.Queries;
using Ogma.Application.UnitTests.Partners.Helpers;

namespace Ogma.Application.UnitTests.Partners.Tests.Queries;

public class GetPartnerByIdHandlerTests
{
    private readonly Mock<IPartnerReader> _partnerReaderStub;
    private readonly GetPartnerByIdHandler _handler;

    public GetPartnerByIdHandlerTests()
    {
        _partnerReaderStub = new Mock<IPartnerReader>();
        _handler = new GetPartnerByIdHandler(_partnerReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsPartner()
    {
        // Arrange
        var partner = PartnersTestData.CreateLegalEntityPartnerDto();
        _partnerReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(partner);
        // Act
        var result = await _handler.Handle(new GetPartnerByIdQuery(partner.Id), CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(partner);
    }

    [Fact]
    public async Task Handle_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var id = PartnersTestData.NextId();
        _partnerReaderStub.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((PartnerDto?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(new GetPartnerByIdQuery(id), CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(id.ToString()));
        _partnerReaderStub.Verify(r => r.GetByIdAsync(id), Times.Once);
    }
}
