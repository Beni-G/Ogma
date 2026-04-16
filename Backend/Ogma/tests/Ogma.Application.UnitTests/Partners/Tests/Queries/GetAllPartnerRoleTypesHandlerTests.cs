using FluentAssertions;
using Moq;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Ports;
using Ogma.Application.Partners.Queries;
using Ogma.Application.UnitTests.Partners.Helpers;

namespace Ogma.Application.UnitTests.Partners.Tests.Queries;

public class GetAllPartnerRoleTypesHandlerTests
{
    private readonly Mock<IPartnerRoleTypeReader> _partnerRoleTypeReaderStub;
    private readonly GetAllPartnerRoleTypesHandler _handler;

    public GetAllPartnerRoleTypesHandlerTests()
    {
        _partnerRoleTypeReaderStub = new Mock<IPartnerRoleTypeReader>();
        _handler = new GetAllPartnerRoleTypesHandler(_partnerRoleTypeReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllPartnerRoleTypes()
    {
        // Arrange
        var partnerRoleTypes = new List<PartnerRoleTypeDto>
        {
            PartnersTestData.CreatePartnerRoleTypeDto(),
            PartnersTestData.CreatePartnerRoleTypeDto()
        };
        _partnerRoleTypeReaderStub.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(partnerRoleTypes);
        // Act
        var result = await _handler.Handle(new GetAllPartnerRoleTypesQuery(), CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(partnerRoleTypes, options => options.WithStrictOrdering());
        _partnerRoleTypeReaderStub.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NoPartnerRoleType_ReturnsEmptyList()
    {
        // Arrange
        var partnerRoleTypes = new List<PartnerRoleTypeDto>();
        _partnerRoleTypeReaderStub.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(partnerRoleTypes);
        // Act
        var result = await _handler.Handle(new GetAllPartnerRoleTypesQuery(), CancellationToken.None);
        // Assert
        result.Should().BeEmpty();
        _partnerRoleTypeReaderStub.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
}
