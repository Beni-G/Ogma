using FluentAssertions;
using Moq;
using Ogma.Application.Partners.Commands;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Extensions;
using Ogma.Application.Partners.Ports;
using Ogma.Application.SharedKernel.Extensions;
using Ogma.Application.UnitTests.Partners.Helpers;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.Partners.Repositories;
using System.Linq.Expressions;

namespace Ogma.Application.UnitTests.Partners.Tests.Commands;

public class UpdatePartnerHandlerTests
{
    private readonly Mock<IPartnerRepository> _partnerRepositoryStub;
    private readonly Mock<IPartnerReader> _partnerReaderStub;
    private readonly Mock<IPartnerRoleTypeReader> _partnerRoleTypeReaderStub;
    private readonly UpdatePartnerHandler _handler;

    public UpdatePartnerHandlerTests()
    {
        _partnerRepositoryStub = new Mock<IPartnerRepository>();
        _partnerReaderStub = new Mock<IPartnerReader>();
        _partnerRoleTypeReaderStub = new Mock<IPartnerRoleTypeReader>();
        _handler = new UpdatePartnerHandler(_partnerRepositoryStub.Object, _partnerReaderStub.Object, _partnerRoleTypeReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ValidPartner_ReturnsUpdatedPartner()
    {
        // Arrange
        var partner = new PartnerBuilder()
            .AsLegalEntity()
            .Build();
        var partnerRole = PartnerRoleType.Reconstitute(partner.RoleIds.First(), "customer", "customer", PartnersTestData.GetMetadata());
        var command = new UpdatePartnerCommandBuilder().FromPartner(partner)
           .TogglingStatus()
           .WithDisplayName("new display name")
           .Build();
        var basePartnerDto = partner.ToDto() with { Roles = new List<PartnerRoleTypeDto> { partnerRole.ToDto() } };
        var expectedPartnerDto = basePartnerDto with
        {
            IsActive = command.IsActive,
            DisplayName = command.DisplayName
        };
        _partnerRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(partner);
        _partnerRoleTypeReaderStub.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<PartnerRoleTypeDto, bool>>>()))
            .ReturnsAsync(new List<PartnerRoleTypeDto> { partnerRole.ToDto() });
        _partnerRepositoryStub.Setup(r => r.UpdateAsync(It.IsAny<Partner>()))
            .ReturnsAsync(true);
        _partnerReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(expectedPartnerDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedPartnerDto);
    }

    [Fact]
    public async Task Handle_NonExistingPartner_ThrowsKeyNotFoundException()
    {
        // Arrange
        var partner = new PartnerBuilder()
            .AsLegalEntity()
            .Build();
        var command = new UpdatePartnerCommandBuilder()
            .FromPartner(partner)
            .TogglingStatus()
            .Build();
        _partnerRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Partner?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(partner.Id.ToString()));
        _partnerRepositoryStub.Verify(r => r.GetByIdAsync(partner.Id), Times.Once);
        _partnerRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<Partner>()), Times.Never);
    }

    [Fact]
    public async Task Handle_NonExistingRoles_ThrowsInvalidOperationException()
    {
        // Arrange
        var partner = new PartnerBuilder()
            .AsLegalEntity()
            .Build();
        var command = new UpdatePartnerCommandBuilder()
            .FromPartner(partner)
            .WithRoles(999L)
            .Build();
        _partnerRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(partner);
        _partnerRoleTypeReaderStub.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<PartnerRoleTypeDto, bool>>>()))
            .ReturnsAsync(new List<PartnerRoleTypeDto>());
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _partnerRoleTypeReaderStub.Verify(r => r.GetAllAsync(It.IsAny<Expression<Func<PartnerRoleTypeDto, bool>>>()), Times.Once);
        _partnerRepositoryStub.Verify(r => r.UpdateAsync(It.IsAny<Partner>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UpdateFails_ThrowsInvalidOperationException()
    {
        // Arrange
        var partner = new PartnerBuilder()
            .AsLegalEntity()
            .Build();
        var partnerRole = PartnerRoleType.Reconstitute(partner.RoleIds.First(), "customer", "customer", PartnersTestData.GetMetadata());
        var command = new UpdatePartnerCommandBuilder().FromPartner(partner)
           .TogglingStatus()
           .WithDisplayName("new display name")
           .Build();
        _partnerRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(partner);
        _partnerRoleTypeReaderStub.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<PartnerRoleTypeDto, bool>>>()))
            .ReturnsAsync(new List<PartnerRoleTypeDto> { partnerRole.ToDto() });
        _partnerRepositoryStub.Setup(r => r.UpdateAsync(It.IsAny<Partner>()))
            .ReturnsAsync(false);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _partnerRepositoryStub.Verify(r => r.UpdateAsync(partner), Times.Once);
    }

    [Fact]
    public async Task Handle_UpdatedPartnerRetrievalFails_ThrowsInvalidOperationException()
    {
        // Arrange
        var partner = new PartnerBuilder()
            .AsLegalEntity()
            .Build();
        var partnerRole = PartnerRoleType.Reconstitute(partner.RoleIds.First(), "customer", "customer", PartnersTestData.GetMetadata());
        var command = new UpdatePartnerCommandBuilder().FromPartner(partner)
           .TogglingStatus()
           .WithDisplayName("new display name")
           .Build();
        _partnerRepositoryStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(partner);
        _partnerRoleTypeReaderStub.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<PartnerRoleTypeDto, bool>>>()))
            .ReturnsAsync(new List<PartnerRoleTypeDto> { partnerRole.ToDto() });
        _partnerRepositoryStub.Setup(r => r.UpdateAsync(It.IsAny<Partner>()))
            .ReturnsAsync(true);
        _partnerReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((PartnerDto?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _partnerReaderStub.Verify(r => r.GetByIdAsync(partner.Id), Times.Once);
    }
}
