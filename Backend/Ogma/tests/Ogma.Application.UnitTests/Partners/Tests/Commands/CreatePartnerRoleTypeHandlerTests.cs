using FluentAssertions;
using Moq;
using Ogma.Application.Partners.Commands;
using Ogma.Application.UnitTests.Partners.Helpers;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.Partners.Repositories;

namespace Ogma.Application.UnitTests.Partners.Tests.Commands;

public class CreatePartnerRoleTypeHandlerTests
{
    private readonly Mock<IPartnerRoleTypeRepository> _partnerRoleTypeRepositoryStub;
    private readonly CreatePartnerRoleTypeHandler _handler;

    public CreatePartnerRoleTypeHandlerTests()
    {
        _partnerRoleTypeRepositoryStub = new Mock<IPartnerRoleTypeRepository>();
        _handler = new CreatePartnerRoleTypeHandler(_partnerRoleTypeRepositoryStub.Object);
    }

    [Fact]
    public async Task Handle_ValidPartnerRoleType_ReturnsCreatedPartnerRoleType()
    {
        // Arrange
        var command = PartnersTestData.GenerateCreatePartnerRoleTypeCommand();
        long nextId = 1L;
        _partnerRoleTypeRepositoryStub.Setup(repo => repo.AddAsync(It.IsAny<PartnerRoleType>()))
            .ReturnsAsync((PartnerRoleType partnerRoleType) =>
            {
                return PartnerRoleType.Reconstitute(nextId, partnerRoleType.Code, partnerRoleType.Name, partnerRoleType.Color);
            });
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(nextId);
        result.Code.Should().Be(command.Code);
        result.Name.Should().Be(command.Name);
        result.Color.Should().Be(command.Color);
        _partnerRoleTypeRepositoryStub.Verify(r => r.AddAsync(It.IsAny<PartnerRoleType>()), Times.Once);
    }
}
