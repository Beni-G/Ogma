using FluentAssertions;
using Moq;
using Ogma.Application.Partners.Commands;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Ports;
using Ogma.Application.SharedKernel.Dtos;
using Ogma.Application.UnitTests.Partners.Helpers;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.Partners.Repositories;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.UnitTests.Partners.Tests.Commands;

public class CreatePartnerHandlerTests
{
    private readonly Mock<IPartnerRepository> _partnerRepositoryStub;
    private readonly Mock<IPartnerReader> _partnerReaderStub;
    private readonly Mock<IPartnerRoleTypeReader> _partnerRoleTypeReaderStub;
    private readonly CreatePartnerHandler _handler;

    public CreatePartnerHandlerTests()
    {
        _partnerRepositoryStub = new Mock<IPartnerRepository>();
        _partnerReaderStub = new Mock<IPartnerReader>();
        _partnerRoleTypeReaderStub = new Mock<IPartnerRoleTypeReader>();
        _handler = new CreatePartnerHandler(_partnerRepositoryStub.Object, _partnerReaderStub.Object, _partnerRoleTypeReaderStub.Object);
    }

    [Fact]
    public async Task Handle_ValidNaturalPersonPartner_ReturnsCreatedPartner()
    {
        // Arrange
        var newPartnerId = PartnersTestData.NextId();
        var newPartnerIdentifierId = PartnersTestData.NextId();
        var partnerRolyType = PartnersTestData.CreatePartnerRoleTypeDto();
        var command = PartnersTestData.GenerateCreatePartnerCommand(true);
        var createdPartner = new PartnerDto(
            newPartnerId,
            new PersonNameDto(command.IndividualName!.FirstName, command.IndividualName.LastName),
            command.CompanyName,
            command.IsNaturalPerson,
            true,
            $"{command.IndividualName.FirstName} {command.IndividualName.LastName}",
            command.HQAddress != null ?
                new AddressDto(
                    command.HQAddress.Street,
                    command.HQAddress.Number,
                    command.HQAddress.City,
                    command.HQAddress.Region,
                    command.HQAddress.PostalCode,
                    command.HQAddress.CountryCode,
                    command.HQAddress.Building,
                    command.HQAddress.Staircase,
                    command.HQAddress.Floor,
                    command.HQAddress.Apartment
                )
                : null,
            new List<PartnerRoleTypeDto> { partnerRolyType },
            new List<PartnerIdentifierDto>
            {
                new PartnerIdentifierDto(newPartnerIdentifierId,
                command.Identifier.Type,
                command.Identifier.Value,
                new PeriodDto(command.Identifier.ValidityPeriod!.Start, command.Identifier.ValidityPeriod.End),
                command.Identifier.IsPrimary)
            },
            new List<PartnerBankAccountDto>(),
            new List<PartnerContactDto>()
            );
        _partnerRoleTypeReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(partnerRolyType);
        _partnerRepositoryStub.Setup(r => r.AddAsync(It.IsAny<Partner>()))
            .ReturnsAsync((Partner partner) =>
            {
                return Partner.Reconstitute(
                    newPartnerId,
                    new PersonName(command.IndividualName!.FirstName, command.IndividualName.LastName),
                    null,
                    command.IsNaturalPerson,
                    true,
                    $"{command.IndividualName!.FirstName} {command.IndividualName.LastName}",
                    command.HQAddress != null ?
                        new Address(
                            command.HQAddress.Street,
                            command.HQAddress.Number,
                            command.HQAddress.City,
                            command.HQAddress.Region,
                            command.HQAddress.PostalCode,
                            command.HQAddress.CountryCode,
                            command.HQAddress.Building,
                            command.HQAddress.Staircase,
                            command.HQAddress.Floor,
                            command.HQAddress.Apartment
                            )
                        : null,
                    new List<PartnerIdentifier>
                    {
                        PartnerIdentifier.Reconstitute(
                            newPartnerIdentifierId,
                            command.Identifier.Type,
                            command.Identifier.Value,
                            PartnersTestData.GetMetadata(),
                            new Period(command.Identifier.ValidityPeriod!.Start, command.Identifier.ValidityPeriod.End),
                            command.Identifier.IsPrimary)
                    },
                    new List<long> { partnerRolyType.Id },
                    new List<PartnerBankAccount>(),
                    new List<PartnerContact>(),
                    PartnersTestData.GetMetadata()
                    );
            });
        _partnerReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(createdPartner);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(createdPartner);
    }

    [Fact]
    public async Task Handle_ValidLegalEntityPartner_ReturnsCreatedPartner()
    {
        // Arrange
        var newPartnerId = PartnersTestData.NextId();
        var newPartnerIdentifierId = PartnersTestData.NextId();
        var partnerRolyType = PartnersTestData.CreatePartnerRoleTypeDto();
        var command = PartnersTestData.GenerateCreatePartnerCommand(false);
        var createdPartner = new PartnerDto(
            newPartnerId,
            null,
            command.CompanyName,
            command.IsNaturalPerson,
            false,
            command.CompanyName,
            command.HQAddress != null ?
                new AddressDto(
                    command.HQAddress.Street,
                    command.HQAddress.Number,
                    command.HQAddress.City,
                    command.HQAddress.Region,
                    command.HQAddress.PostalCode,
                    command.HQAddress.CountryCode,
                    command.HQAddress.Building,
                    command.HQAddress.Staircase,
                    command.HQAddress.Floor,
                    command.HQAddress.Apartment
                )
                : null,
            new List<PartnerRoleTypeDto> { partnerRolyType },
            new List<PartnerIdentifierDto>
            {
                new PartnerIdentifierDto(
                    newPartnerIdentifierId,
                    command.Identifier.Type,
                    command.Identifier.Value,
                    new PeriodDto(command.Identifier.ValidityPeriod!.Start, command.Identifier.ValidityPeriod.End),
                    command.Identifier.IsPrimary)
            },
            new List<PartnerBankAccountDto>(),
            new List<PartnerContactDto>()
            );
        _partnerRoleTypeReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(partnerRolyType);
        _partnerRepositoryStub.Setup(r => r.AddAsync(It.IsAny<Partner>()))
            .ReturnsAsync((Partner partner) =>
            {
                return Partner.Reconstitute(
                    newPartnerId,
                    null,
                    command.CompanyName,
                    command.IsNaturalPerson,
                    true,
                    command.CompanyName,
                    command.HQAddress != null ?
                        new Address(
                            command.HQAddress.Street,
                            command.HQAddress.Number,
                            command.HQAddress.City,
                            command.HQAddress.Region,
                            command.HQAddress.PostalCode,
                            command.HQAddress.CountryCode,
                            command.HQAddress.Building,
                            command.HQAddress.Staircase,
                            command.HQAddress.Floor,
                            command.HQAddress.Apartment
                            )
                        : null,
                    new List<PartnerIdentifier>
                    {
                        PartnerIdentifier.Reconstitute(
                            newPartnerIdentifierId,
                            command.Identifier.Type,
                            command.Identifier.Value,
                            PartnersTestData.GetMetadata(),
                            new Period(command.Identifier.ValidityPeriod!.Start, command.Identifier.ValidityPeriod.End),
                            command.Identifier.IsPrimary)
                    },
                    new List<long> { partnerRolyType.Id },
                    new List<PartnerBankAccount>(),
                    new List<PartnerContact>(),
                    PartnersTestData.GetMetadata()
                    );
            });
        _partnerReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(createdPartner);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        result.Should().BeEquivalentTo(createdPartner);
    }

    [Fact]
    public async Task Handle_PartnerRoleTypeNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = PartnersTestData.GenerateCreatePartnerCommand();
        _partnerRoleTypeReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((PartnerRoleTypeDto?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .Where(ex => ex.Message.Contains(command.RoleId.ToString()));
        _partnerRepositoryStub.Verify(r => r.AddAsync(It.IsAny<Partner>()), Times.Never);
    }

    [Fact]
    public async Task Handle_RetrievalOfCreatedPartnerFails_ThrowsInvalidOperationException()
    {
        // Arrange
        var newPartnerId = PartnersTestData.NextId();
        var newPartnerIdentifierId = PartnersTestData.NextId();
        var partnerRolyType = PartnersTestData.CreatePartnerRoleTypeDto();
        var command = PartnersTestData.GenerateCreatePartnerCommand(false);
        var createdPartner = new PartnerDto(
            newPartnerId,
            null,
            command.CompanyName,
            command.IsNaturalPerson,
            false,
            command.CompanyName,
            command.HQAddress != null ?
                new AddressDto(
                    command.HQAddress.Street,
                    command.HQAddress.Number,
                    command.HQAddress.City,
                    command.HQAddress.Region,
                    command.HQAddress.PostalCode,
                    command.HQAddress.CountryCode,
                    command.HQAddress.Building,
                    command.HQAddress.Staircase,
                    command.HQAddress.Floor,
                    command.HQAddress.Apartment
                )
                : null,
            new List<PartnerRoleTypeDto> { partnerRolyType },
            new List<PartnerIdentifierDto>
            {
                new PartnerIdentifierDto(
                    newPartnerIdentifierId,
                    command.Identifier.Type,
                    command.Identifier.Value,
                    new PeriodDto(command.Identifier.ValidityPeriod!.Start, command.Identifier.ValidityPeriod.End),
                    command.Identifier.IsPrimary)
            },
            new List<PartnerBankAccountDto>(),
            new List<PartnerContactDto>()
            );
        _partnerRoleTypeReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(partnerRolyType);
        _partnerRepositoryStub.Setup(r => r.AddAsync(It.IsAny<Partner>()))
            .ReturnsAsync((Partner partner) =>
            {
                return Partner.Reconstitute(
                    newPartnerId,
                    null,
                    command.CompanyName,
                    command.IsNaturalPerson,
                    true,
                    command.CompanyName,
                    command.HQAddress != null ?
                        new Address(
                            command.HQAddress.Street,
                            command.HQAddress.Number,
                            command.HQAddress.City,
                            command.HQAddress.Region,
                            command.HQAddress.PostalCode,
                            command.HQAddress.CountryCode,
                            command.HQAddress.Building,
                            command.HQAddress.Staircase,
                            command.HQAddress.Floor,
                            command.HQAddress.Apartment
                            )
                        : null,
                    new List<PartnerIdentifier>
                    {
                        PartnerIdentifier.Reconstitute(
                            newPartnerIdentifierId,
                            command.Identifier.Type,
                            command.Identifier.Value,
                            PartnersTestData.GetMetadata(),
                            new Period(command.Identifier.ValidityPeriod!.Start, command.Identifier.ValidityPeriod.End),
                            command.Identifier.IsPrimary)
                    },
                    new List<long> { partnerRolyType.Id },
                    new List<PartnerBankAccount>(),
                    new List<PartnerContact>(),
                    PartnersTestData.GetMetadata()
                    );
            });
        _partnerReaderStub.Setup(r => r.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((PartnerDto?)null);
        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .Where(ex => ex.Message.Contains(createdPartner.Id.ToString()));
    }
}
