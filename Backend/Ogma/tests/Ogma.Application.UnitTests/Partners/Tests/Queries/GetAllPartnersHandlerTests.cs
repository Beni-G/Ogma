using FluentAssertions;
using Moq;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Ports;
using Ogma.Application.Partners.Queries;
using Ogma.Application.UnitTests.Partners.Helpers;

namespace Ogma.Application.UnitTests.Partners.Tests.Queries;

public class GetAllPartnersHandlerTests
{
    private readonly Mock<IPartnerReader> _partnerReaderStub;
    private readonly GetAllPartnersHandler _handler;

    public GetAllPartnersHandlerTests()
    {
        _partnerReaderStub = new Mock<IPartnerReader>();
        _handler = new GetAllPartnersHandler(_partnerReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllPartners()
    {
        // Arrange
        var partners = new List<PartnerDto>
        {
            PartnersTestData.CreateLegalEntityPartnerDto(),
            PartnersTestData.CreateNaturalPersonPartnerDto()
        };
        _partnerReaderStub.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(partners);
        // Act
        var result = await _handler.Handle(new GetAllPartnersQuery(), CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(partners, options => options.WithStrictOrdering());
        _partnerReaderStub.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NoPartners_ReturnsEmptyList()
    {
        // Arrange
        var partners = new List<PartnerDto>();
        _partnerReaderStub.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(partners);
        // Act
        var result = await _handler.Handle(new GetAllPartnersQuery(), CancellationToken.None);
        // Assert
        result.Should().BeEmpty();
        _partnerReaderStub.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
}
