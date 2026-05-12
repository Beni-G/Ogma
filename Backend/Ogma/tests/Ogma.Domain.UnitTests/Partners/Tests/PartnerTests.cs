using FluentAssertions;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;
using Ogma.Domain.UnitTests.Partners.Helpers;

namespace Ogma.Domain.UnitTests.Partners.Tests;

public class PartnerTests
{
    private readonly Address _defaultAddress;

    public PartnerTests()
    {
        _defaultAddress = new Address(
            "Main St.",
            "1B",
            "Gotham",
            "Gotham State",
            "987654",
            "US",
            "Gotham Tower",
            "D",
            "99",
            "99F");
    }
    [Fact]
    public void CreateIndividual_ValidParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var individualName = new PersonName("Jane", "Doe");
        var partnerIdentifier = PartnerIdentifier.Create("SSN", "123-45-6789");
        var partnerRoleId = 1L;
        // Act
        var partner = Partner.CreateIndividual(individualName, partnerIdentifier, partnerRoleId, _defaultAddress);
        // Assert
        partner.IndividualName.Should().Be(individualName);
        partner.CompanyName.Should().BeNull();
        partner.IsNaturalPerson.Should().BeTrue();
        partner.Identifiers.Should().ContainSingle().Which.Should().Be(partnerIdentifier);
        partner.Identifiers.FirstOrDefault()?.IsPrimary.Should().BeTrue();
        partner.RoleIds.Should().ContainSingle().Which.Should().Be(partnerRoleId);
        partner.HQAddress.Should().Be(_defaultAddress);
    }

    [Fact]
    public void CreateIndividual_OptionalAddressNotProvided_SetsHQAddressToNull()
    {
        // Arrange
        var individualName = new PersonName("Jane", "Doe");
        var partnerIdentifier = PartnerIdentifier.Create("SSN", "123-45-6789");
        var partnerRoleId = 1L;
        // Act
        var partner = Partner.CreateIndividual(individualName, partnerIdentifier, partnerRoleId);
        // Assert
        partner.HQAddress.Should().BeNull();
    }

    [Fact]
    public void CreateIndividual_NullName_ThrowsArgumentNullException()
    {
        // Arrange
        var partnerIdentifier = PartnerIdentifier.Create("SSN", "123-45-6789");
        var partnerRoleId = 1L;
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Partner.CreateIndividual(null!, partnerIdentifier, partnerRoleId));
    }

    [Fact]
    public void CreateIndividual_NullIdentifier_ThrowsArgumentNullException()
    {
        // Arrange
        var individualName = new PersonName("Jane", "Doe");
        var partnerRoleId = 1L;
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Partner.CreateIndividual(individualName, null!, partnerRoleId));
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void CreateIndividual_InvalidRoleId_ThrowsArgumentException(long invalidRoleId)
    {
        // Arrange
        var individualName = new PersonName("Jane", "Doe");
        var partnerIdentifier = PartnerIdentifier.Create("SSN", "123-45-6789");
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Partner.CreateIndividual(individualName, partnerIdentifier, invalidRoleId));
    }

    [Fact]
    public void CreateCompany_ValidParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var companyName = "Acme Corp";
        var partnerIdentifier = PartnerIdentifier.Create("EIN", "12-3456789");
        var partnerRoleId = 1L;
        // Act
        var partner = Partner.CreateLegalEntity(companyName, partnerIdentifier, partnerRoleId, _defaultAddress);
        // Assert
        partner.CompanyName.Should().Be(companyName);
        partner.IndividualName.Should().BeNull();
        partner.IsNaturalPerson.Should().BeFalse();
        partner.Identifiers.Should().ContainSingle().Which.Should().Be(partnerIdentifier);
        partner.Identifiers.FirstOrDefault()?.IsPrimary.Should().BeTrue();
        partner.RoleIds.Should().ContainSingle().Which.Should().Be(partnerRoleId);
        partner.HQAddress.Should().Be(_defaultAddress);
    }

    [Fact]
    public void CreateCompany_OptionalAddressNotProvided_SetsHQAddressToNull()
    {
        // Arrange
        var companyName = "Acme Corp";
        var partnerIdentifier = PartnerIdentifier.Create("EIN", "12-3456789");
        var partnerRoleId = 1L;
        // Act
        var partner = Partner.CreateLegalEntity(companyName, partnerIdentifier, partnerRoleId);
        // Assert
        partner.HQAddress.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateCompany_InvalidName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var partnerIdentifier = PartnerIdentifier.Create("EIN", "12-3456789");
        var partnerRoleId = 1L;
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Partner.CreateLegalEntity(invalidName!, partnerIdentifier, partnerRoleId));
    }

    [Fact]
    public void CreateCompany_NullIdentifier_ThrowsArgumentNullException()
    {
        // Arrange
        var companyName = "Acme Corp";
        var partnerRoleId = 1L;
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Partner.CreateLegalEntity(companyName, null!, partnerRoleId));
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void CreateCompany_InvalidRoleId_ThrowsArgumentException(long invalidRoleId)
    {
        // Arrange
        var companyName = "Acme Corp";
        var partnerIdentifier = PartnerIdentifier.Create("EIN", "12-3456789");
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Partner.CreateLegalEntity(companyName, partnerIdentifier, invalidRoleId));
    }

    [Fact]
    public void Reconstitute_ValidParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        long id = 5;
        var individualName = new PersonName("Alice", "Smith");
        string? companyName = null;
        bool isNaturalPerson = true;
        bool isActive = true;
        string? displayName = "Alice S.";
        var identifiers = new List<PartnerIdentifier>
        {
            PartnerIdentifier.Create("SSN", "987-65-4321", isPrimary: true)
        };
        var roleIds = new List<long> { 1L, 2L };
        var bankAccounts = new List<PartnerBankAccount>();
        var contacts = new List<PartnerContact>();
        // Act
        var partner = Partner.Reconstitute(id, individualName, companyName, isNaturalPerson, isActive, displayName, _defaultAddress, identifiers, roleIds, bankAccounts, contacts, PartnersTestData.GetMetadata());
        // Assert
        partner.Id.Should().Be(id);
        partner.IndividualName.Should().Be(individualName);
        partner.CompanyName.Should().BeNull();
        partner.IsNaturalPerson.Should().BeTrue();
        partner.IsActive.Should().BeTrue();
        partner.DisplayName.Should().Be(displayName);
        partner.HQAddress.Should().Be(_defaultAddress);
        partner.Identifiers.Should().BeEquivalentTo(identifiers);
        partner.RoleIds.Should().BeEquivalentTo(roleIds);
        partner.BankAccounts.Should().BeEmpty();
        partner.Contacts.Should().BeEmpty();
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    public void Reconstitute_InvalidId_ThrowsArgumentException(long invalidId)
    {
        // Arrange
        var individualName = new PersonName("Alice", "Smith");
        string? companyName = "Acme Corp";
        bool isNaturalPerson = true;
        bool isActive = true;
        string? displayName = "Alice S.";
        Address? mainAddress = null;
        var identifiers = new List<PartnerIdentifier>
        {
            PartnerIdentifier.Create("SSN", "987-65-4321", isPrimary: true)
        };
        var roleIds = new List<long> { 1L, 2L };
        var bankAccounts = new List<PartnerBankAccount>();
        var contacts = new List<PartnerContact>();
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            Partner.Reconstitute(invalidId, individualName, companyName, isNaturalPerson, isActive, displayName, mainAddress, identifiers, roleIds, bankAccounts, contacts, PartnersTestData.GetMetadata()));
    }

    [Fact]
    public void Reconstitute_BothInvidualAndCompanyNameProvided_ThrowsInvalidOperationException()
    {
        // Arrange
        long id = 5;
        var individualName = new PersonName("Alice", "Smith");
        string? companyName = "Acme Corp";
        bool isNaturalPerson = true;
        bool isActive = true;
        string? displayName = "Alice S.";
        Address? mainAddress = null;
        var identifiers = new List<PartnerIdentifier>
        {
            PartnerIdentifier.Create("SSN", "987-65-4321", isPrimary: true)
        };
        var roleIds = new List<long> { 1L, 2L };
        var bankAccounts = new List<PartnerBankAccount>();
        var contacts = new List<PartnerContact>();
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            Partner.Reconstitute(id, individualName, companyName, isNaturalPerson, isActive, displayName, mainAddress, identifiers, roleIds, bankAccounts, contacts, PartnersTestData.GetMetadata()));
    }

    [Fact]
    public void Reconstitute_NeitherInvidualNorCompanyNameProvided_ThrowsArgumentException()
    {
        // Arrange
        long id = 5;
        bool isNaturalPerson = true;
        bool isActive = true;
        string? displayName = "Alice S.";
        Address? mainAddress = null;
        var identifiers = new List<PartnerIdentifier>
        {
            PartnerIdentifier.Create("SSN", "987-65-4321", isPrimary: true)
        };
        var roleIds = new List<long> { 1L, 2L };
        var bankAccounts = new List<PartnerBankAccount>();
        var contacts = new List<PartnerContact>();
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            Partner.Reconstitute(id, null, null, isNaturalPerson, isActive, displayName, mainAddress, identifiers, roleIds, bankAccounts, contacts, PartnersTestData.GetMetadata()));
    }

    [Fact]
    public void Reconstitute_IndividualNameWithNotIsNaturalPerson_ThrowsInvalidOperationException()
    {
        // Arrange
        long id = 5;
        var individualName = new PersonName("Alice", "Smith");
        string? companyName = null;
        bool isNaturalPerson = false; // Inconsistent
        bool isActive = true;
        string? displayName = "Alice S.";
        Address? mainAddress = null;
        var identifiers = new List<PartnerIdentifier>
        {
            PartnerIdentifier.Create("SSN", "987-65-4321", isPrimary: true)
        };
        var roleIds = new List<long> { 1L, 2L };
        var bankAccounts = new List<PartnerBankAccount>();
        var contacts = new List<PartnerContact>();
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            Partner.Reconstitute(id, individualName, companyName, isNaturalPerson, isActive, displayName, mainAddress, identifiers, roleIds, bankAccounts, contacts, PartnersTestData.GetMetadata()));
    }

    [Fact]
    public void Reconstitute_CompanyNameWithIsNaturalPerson_ThrowsInvalidOperationException()
    {
        // Arrange
        long id = 5;
        PersonName? individualName = null;
        string? companyName = "Acme Corp";
        bool isNaturalPerson = true; // Inconsistent
        bool isActive = true;
        string? displayName = "Acme";
        Address? mainAddress = null;
        var identifiers = new List<PartnerIdentifier>
        {
            PartnerIdentifier.Create("EIN", "12-3456789", isPrimary: true)
        };
        var roleIds = new List<long> { 1L, 2L };
        var bankAccounts = new List<PartnerBankAccount>();
        var contacts = new List<PartnerContact>();
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            Partner.Reconstitute(id, individualName, companyName, isNaturalPerson, isActive, displayName, mainAddress, identifiers, roleIds, bankAccounts, contacts, PartnersTestData.GetMetadata()));
    }

    [Fact]
    public void Reconstitute_NullIdentifiers_ThrowsArgumentNullException()
    {
        // Arrange
        long id = 5;
        var individualName = new PersonName("Alice", "Smith");
        string? companyName = null;
        bool isNaturalPerson = true;
        bool isActive = true;
        string? displayName = "Alice S.";
        Address? mainAddress = null;
        var roleIds = new List<long> { 1L, 2L };
        var bankAccounts = new List<PartnerBankAccount>();
        var contacts = new List<PartnerContact>();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            Partner.Reconstitute(id, individualName, companyName, isNaturalPerson, isActive, displayName, mainAddress, null!, roleIds, bankAccounts, contacts, PartnersTestData.GetMetadata()));
    }

    [Fact]
    public void Reconstitute_EmptyIdentifiers_ThrowsArgumentException()
    {
        // Arrange
        long id = 5;
        var individualName = new PersonName("Alice", "Smith");
        string? companyName = null;
        bool isNaturalPerson = true;
        bool isActive = true;
        string? displayName = "Alice S.";
        Address? mainAddress = null;
        var identifiers = new List<PartnerIdentifier>();
        var roleIds = new List<long> { 1L, 2L };
        var bankAccounts = new List<PartnerBankAccount>();
        var contacts = new List<PartnerContact>();
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            Partner.Reconstitute(id, individualName, companyName, isNaturalPerson, isActive, displayName, mainAddress, identifiers, roleIds, bankAccounts, contacts, PartnersTestData.GetMetadata()));
    }

    [Fact]
    public void Reconstitute_NullRoles_ThrowsArgumentNullException()
    {
        // Arrange
        long id = 5;
        var individualName = new PersonName("Alice", "Smith");
        string? companyName = null;
        bool isNaturalPerson = true;
        bool isActive = true;
        string? displayName = "Alice S.";
        Address? mainAddress = null;
        var identifiers = new List<PartnerIdentifier>
        {
            PartnerIdentifier.Create("SSN", "987-65-4321", isPrimary: true)
        };
        var bankAccounts = new List<PartnerBankAccount>();
        var contacts = new List<PartnerContact>();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            Partner.Reconstitute(id, individualName, companyName, isNaturalPerson, isActive, displayName, mainAddress, identifiers, null!, bankAccounts, contacts, PartnersTestData.GetMetadata()));
    }

    [Fact]
    public void Reconstitute_EmptyRoles_ThrowsArgumentException()
    {
        // Arrange
        long id = 5;
        var individualName = new PersonName("Alice", "Smith");
        string? companyName = null;
        bool isNaturalPerson = true;
        bool isActive = true;
        string? displayName = "Alice S.";
        Address? mainAddress = null;
        var identifiers = new List<PartnerIdentifier>
        {
            PartnerIdentifier.Create("SSN", "987-65-4321", isPrimary: true)
        };
        var roleIds = new List<long>();
        var bankAccounts = new List<PartnerBankAccount>();
        var contacts = new List<PartnerContact>();
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            Partner.Reconstitute(id, individualName, companyName, isNaturalPerson, isActive, displayName, mainAddress, identifiers, roleIds, bankAccounts, contacts, PartnersTestData.GetMetadata()));
    }

    [Fact]
    public void Reconstitute_NullBankAccounts_ThrowsArgumentNullException()
    {
        // Arrange
        long id = 5;
        var individualName = new PersonName("Alice", "Smith");
        string? companyName = null;
        bool isNaturalPerson = true;
        bool isActive = true;
        string? displayName = "Alice S.";
        Address? mainAddress = null;
        var identifiers = new List<PartnerIdentifier>
        {
            PartnerIdentifier.Create("SSN", "987-65-4321", isPrimary: true)
        };
        var roleIds = new List<long> { 1L, 2L };
        var contacts = new List<PartnerContact>();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            Partner.Reconstitute(id, individualName, companyName, isNaturalPerson, isActive, displayName, mainAddress, identifiers, roleIds, null!, contacts, PartnersTestData.GetMetadata()));
    }

    [Fact]
    public void Reconstitute_NullContacts_ThrowsArgumentNullException()
    {
        // Arrange
        long id = 5;
        var individualName = new PersonName("Alice", "Smith");
        string? companyName = null;
        bool isNaturalPerson = true;
        bool isActive = true;
        string? displayName = "Alice S.";
        Address? mainAddress = null;
        var identifiers = new List<PartnerIdentifier>
        {
            PartnerIdentifier.Create("SSN", "987-65-4321", isPrimary: true)
        };
        var roleIds = new List<long> { 1L, 2L };
        var bankAccounts = new List<PartnerBankAccount>();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            Partner.Reconstitute(id, individualName, companyName, isNaturalPerson, isActive, displayName, mainAddress, identifiers, roleIds, bankAccounts, null!, PartnersTestData.GetMetadata()));
    }

    [Fact]
    public void Update_ValidParametersForIndividualStatusUnchanged_UpdatesPropertiesCorrectly()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            new PersonName("Old", "Name"),
            null,
            true,
            true,
            "Oldie",
            new Address("Second St", "123", "City", "State", "12345", "US"),
            new List<PartnerIdentifier>
            {
                PartnerIdentifier.Create("SSN", "000-00-0000")
            },
            new List<long> { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());

        var newIndividualName = new PersonName("New", "Name");
        string newDisplayName = "New Name";
        bool newIsActive = false;
        List<PartnerIdentifier> newIdentifiers = new()
        {
            PartnerIdentifier.Create("SSN", "111-11-1111", isPrimary: true)
        };
        List<long> newRoleIds = new() { 2L, 3L };
        List<PartnerBankAccount> newBankAccounts = new()
        {
            PartnerBankAccount.Create(new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX"))
        };
        List<PartnerContact> newContacts = new()
        {
            PartnerContact.Create(new PersonName("Jon", "Snow"))
        };
        // Act
        partner.Update(
            newIndividualName,
            null,
            true,
            newIsActive,
            newDisplayName,
            _defaultAddress,
            newIdentifiers,
            newRoleIds,
            newBankAccounts,
            newContacts);
        // Assert
        partner.IndividualName.Should().Be(newIndividualName);
        partner.CompanyName.Should().BeNull();
        partner.IsNaturalPerson.Should().BeTrue();
        partner.IsActive.Should().Be(newIsActive);
        partner.DisplayName.Should().Be(newDisplayName);
        partner.HQAddress.Should().Be(_defaultAddress);
        partner.Identifiers.Should().BeEquivalentTo(newIdentifiers);
        partner.RoleIds.Should().BeEquivalentTo(newRoleIds);
        partner.BankAccounts.Should().BeEquivalentTo(newBankAccounts);
        partner.Contacts.Should().BeEquivalentTo(newContacts);
    }

    [Fact]
    public void Update_ValidParametersForLegalEntitiesStatusUnchanged_UpdatesPropertiesCorrectly()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            null,
            "Firma Mare SA",
            false,
            true,
            "Biggy",
            new Address("Strada Foamei", "123", "City", "State", "12345", "RO"),
            new List<PartnerIdentifier>
            {
                PartnerIdentifier.Create("CUI", "000-00-0000")
            },
            new List<long> { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());

        var newCompanyName = "Firma Medie SRL";
        string newDisplayName = "Mediumy";
        bool newIsActive = false;
        List<PartnerIdentifier> newIdentifiers = new()
        {
            PartnerIdentifier.Create("CUI", "111-11-1111", isPrimary: true)
        };
        List<long> newRoleIds = new() { 2L, 3L };
        List<PartnerBankAccount> newBankAccounts = new()
        {
            PartnerBankAccount.Create(new BankAccount("Big Bank", "DE12 5001 0517 0648 4898 90", "eur", "COBADEFFXXX"))
        };
        List<PartnerContact> newContacts = new()
        {
            PartnerContact.Create(new PersonName("Jon", "Snow"))
        };
        // Act
        partner.Update(
            null,
            newCompanyName,
            false,
            newIsActive,
            newDisplayName,
            _defaultAddress,
            newIdentifiers,
            newRoleIds,
            newBankAccounts,
            newContacts);
        // Assert
        partner.IndividualName.Should().BeNull();
        partner.CompanyName.Should().Be(newCompanyName);
        partner.IsNaturalPerson.Should().BeFalse();
        partner.IsActive.Should().Be(newIsActive);
        partner.DisplayName.Should().Be(newDisplayName);
        partner.HQAddress.Should().Be(_defaultAddress);
        partner.Identifiers.Should().BeEquivalentTo(newIdentifiers);
        partner.RoleIds.Should().BeEquivalentTo(newRoleIds);
        partner.BankAccounts.Should().BeEquivalentTo(newBankAccounts);
        partner.Contacts.Should().BeEquivalentTo(newContacts);
    }

    [Fact]
    public void Update_IndividualToLegalEntityChange_UpdatesCorrectly()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            new PersonName("John", "Doe"),
            null,
            true,
            true,
            null,
            null,
            new List<PartnerIdentifier>()
            {
                PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
            },
            new List<long>() { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());
        var newCompanyName = "Acme Corp";
        // Act
        partner.Update(
            null,
            newCompanyName,
            false,
            true,
            null,
            null,
            new List<PartnerIdentifier>()
            {
                PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
            },
            new List<long>() { 2L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>());
        // Assert
        partner.IndividualName.Should().BeNull();
        partner.CompanyName.Should().Be(newCompanyName);
        partner.IsNaturalPerson.Should().BeFalse();
    }

    [Fact]
    public void Update_LegalEntityToIndividualChange_UpdatesCorrectly()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            null,
            "Acme Corp",
            false,
            true,
            null,
            null,
            new List<PartnerIdentifier>()
            {
                PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
            },
            new List<long>() { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());
        var newIndividualName = new PersonName("John", "Doe");
        // Act
        partner.Update(
            newIndividualName,
            null,
            true,
            true,
            null,
            null,
            new List<PartnerIdentifier>()
            {
                PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
            },
            new List<long>() { 2L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>());
        // Assert
        partner.CompanyName.Should().BeNull();
        partner.IndividualName.Should().Be(newIndividualName);
        partner.IsNaturalPerson.Should().BeTrue();
    }

    [Fact]
    public void Update_ValidParameters_UpdatesMetadataCorrectly()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            null,
            "Acme Corp",
            false,
            true,
            null,
            null,
            new List<PartnerIdentifier>()
            {
                PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
            },
            new List<long>() { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());
        var oldPartnerMetadata = new EntityMetadata(partner.Metadata.CreatedAt, partner.Metadata.UpdatedAt, partner.Metadata.Version);
        var newIndividualName = new PersonName("John", "Doe");
        // Act
        partner.Update(
            newIndividualName,
            null,
            true,
            true,
            null,
            null,
            new List<PartnerIdentifier>()
            {
                PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
            },
            new List<long>() { 2L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>());
        // Assert
        partner.Metadata.CreatedAt.Should().Be(oldPartnerMetadata.CreatedAt);
        partner.Metadata.UpdatedAt.Should().BeAfter(oldPartnerMetadata.UpdatedAt);
        partner.Metadata.Version.Should().Be(oldPartnerMetadata.Version + 1);
    }

    [Fact]
    public void Update_IsNaturalPersonNoIndividualName_ThrowsInvalidOperationException()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            null,
            "Acme Corp",
            false,
            true,
            null,
            null,
            new List<PartnerIdentifier>()
            {
                PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
            },
            new List<long>() { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            partner.Update(
                null,
                null,
                true,
                true,
                null,
                null,
                new List<PartnerIdentifier>()
                {
                    PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
                },
                new List<long>() { 2L },
                new List<PartnerBankAccount>(),
                new List<PartnerContact>()));
    }

    [Fact]
    public void Update_NotIsNaturalPersonNoCompanyName_ThrowsInvalidOperationException()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            new PersonName("John", "Doe"),
            null,
            true,
            true,
            null,
            null,
            new List<PartnerIdentifier>()
            {
                PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
            },
            new List<long>() { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            partner.Update(
                null,
                null,
                false,
                true,
                null,
                null,
                new List<PartnerIdentifier>()
                {
                    PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
                },
                new List<long>() { 2L },
                new List<PartnerBankAccount>(),
                new List<PartnerContact>()));
    }

    [Fact]
    public void Update_BothCompanyAndIndividualNameProvided_ThrowsInvalidOperationException()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            new PersonName("John", "Doe"),
            null,
            true,
            true,
            null,
            null,
            new List<PartnerIdentifier>()
            {
                PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
            },
            new List<long>() { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            partner.Update(
                new PersonName("John", "Doe"),
                "Acme GMBH",
                false,
                true,
                null,
                null,
                new List<PartnerIdentifier>()
                {
                    PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
                },
                new List<long>() { 2L },
                new List<PartnerBankAccount>(),
                new List<PartnerContact>()));
    }

    [Fact]
    public void Update_NeitherCompanyNorIndividualNameProvided_ThrowsInvalidOperationException()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            new PersonName("John", "Doe"),
            null,
            true,
            true,
            null,
            null,
            new List<PartnerIdentifier>()
            {
                PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
            },
            new List<long>() { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            partner.Update(
                null,
                null,
                false,
                true,
                null,
                null,
                new List<PartnerIdentifier>()
                {
                    PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
                },
                new List<long>() { 2L },
                new List<PartnerBankAccount>(),
                new List<PartnerContact>()));
    }

    [Fact]
    public void Update_IndividualNameSetAndNotIsNaturalPerson_ThrowsInvalidOperationException()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            null,
            "Acme Corp",
            false,
            true,
            null,
            null,
            new List<PartnerIdentifier>()
            {
                PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
            },
            new List<long>() { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            partner.Update(
                new PersonName("Jane", "Doe"),
                null,
                false,
                true,
                null,
                null,
                new List<PartnerIdentifier>()
                {
                    PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
                },
                new List<long>() { 2L },
                new List<PartnerBankAccount>(),
                new List<PartnerContact>()));
    }

    [Fact]
    public void Update_CompanyNameSetAndIsNaturalPerson_ThrowsInvalidOperationException()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            new PersonName("Jane", "Doe"),
            null,
            true,
            true,
            null,
            null,
            new List<PartnerIdentifier>()
            {
                PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
            },
            new List<long>() { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            partner.Update(
                null,
                "Acme Corp",
                true,
                true,
                null,
                null,
                new List<PartnerIdentifier>()
                {
                    PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
                },
                new List<long>() { 2L },
                new List<PartnerBankAccount>(),
                new List<PartnerContact>()));
    }

    [Fact]
    public void Update_NoIdentifiersProvided_ThrowsInvalidOperationException()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            new PersonName("Old", "Name"),
            null,
            true,
            true,
            "Oldie",
            new Address("Second St", "123", "City", "State", "12345", "US"),
            new List<PartnerIdentifier>
            {
                PartnerIdentifier.Create("SSN", "000-00-0000")
            },
            new List<long>() { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());

        var newIndividualName = new PersonName("New", "Name");
        string newDisplayName = "New Name";
        Address newMainAddress = new Address("Main St", "123", "City", "State", "12345", "US");
        bool newIsActive = false;
        List<PartnerIdentifier> newIdentifiers = new();
        List<long> newRoleIds = new() { 2L, 3L };
        List<PartnerBankAccount> newBankAccounts = new();
        List<PartnerContact> newContacts = new();
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => partner.Update(
            newIndividualName,
            null,
            true,
            newIsActive,
            newDisplayName,
            newMainAddress,
            newIdentifiers,
            newRoleIds,
            newBankAccounts,
            newContacts));
    }

    [Fact]
    public void Update_NoRolesProvided_ThrowsInvalidOperationException()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            new PersonName("Old", "Name"),
            null,
            true,
            true,
            "Oldie",
            new Address("Second St", "123", "City", "State", "12345", "US"),
            new List<PartnerIdentifier>
            {
                PartnerIdentifier.Create("SSN", "000-00-0000")
            },
            new List<long>() { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());

        var newIndividualName = new PersonName("New", "Name");
        string newDisplayName = "New Name";
        Address newMainAddress = new Address("Main St", "123", "City", "State", "12345", "US");
        bool newIsActive = false;
        List<PartnerIdentifier> newIdentifiers = new()
        {
            PartnerIdentifier.Create("SSN", "000-00-0000")
        };
        List<long> newRoleIds = new();
        List<PartnerBankAccount> newBankAccounts = new();
        List<PartnerContact> newContacts = new();
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => partner.Update(
            newIndividualName,
            null,
            true,
            newIsActive,
            newDisplayName,
            newMainAddress,
            newIdentifiers,
            newRoleIds,
            newBankAccounts,
            newContacts));
    }

    [Fact]
    public void Update_NullIdentifiers_ThrowsInvalidOperationException()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            new PersonName("Old", "Name"),
            null,
            true,
            true,
            "Oldie",
            new Address("Second St", "123", "City", "State", "12345", "US"),
            new List<PartnerIdentifier>
            {
                PartnerIdentifier.Create("SSN", "000-00-0000")
            },
            new List<long>() { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());

        var newIndividualName = new PersonName("New", "Name");
        string newDisplayName = "New Name";
        Address newMainAddress = new Address("Main St", "123", "City", "State", "12345", "US");
        bool newIsActive = false;
        List<long> newRoleIds = new();
        List<PartnerBankAccount> newBankAccounts = new();
        List<PartnerContact> newContacts = new();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => partner.Update(
            newIndividualName,
            null,
            true,
            newIsActive,
            newDisplayName,
            newMainAddress,
            null,
            newRoleIds,
            newBankAccounts,
            newContacts));
    }

    [Fact]
    public void Update_NullRoles_ThrowsInvalidOperationException()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            new PersonName("Old", "Name"),
            null,
            true,
            true,
            "Oldie",
            new Address("Second St", "123", "City", "State", "12345", "US"),
            new List<PartnerIdentifier>
            {
                PartnerIdentifier.Create("SSN", "000-00-0000")
            },
            new List<long>() { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());

        var newIndividualName = new PersonName("New", "Name");
        string newDisplayName = "New Name";
        Address newMainAddress = new Address("Main St", "123", "City", "State", "12345", "US");
        bool newIsActive = false;
        List<PartnerIdentifier> newIdentifiers = new();
        List<PartnerBankAccount> newBankAccounts = new();
        List<PartnerContact> newContacts = new();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => partner.Update(
            newIndividualName,
            null,
            true,
            newIsActive,
            newDisplayName,
            newMainAddress,
            newIdentifiers,
            null,
            newBankAccounts,
            newContacts));
    }

    [Fact]
    public void Update_NullBankAccounts_ThrowsInvalidOperationException()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            new PersonName("Old", "Name"),
            null,
            true,
            true,
            "Oldie",
            new Address("Second St", "123", "City", "State", "12345", "US"),
            new List<PartnerIdentifier>
            {
                PartnerIdentifier.Create("SSN", "000-00-0000")
            },
            new List<long>() { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());

        var newIndividualName = new PersonName("New", "Name");
        string newDisplayName = "New Name";
        Address newMainAddress = new Address("Main St", "123", "City", "State", "12345", "US");
        bool newIsActive = false;
        List<PartnerIdentifier> newIdentifiers = new();
        List<long> newRoleIds = new();
        List<PartnerContact> newContacts = new();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => partner.Update(
            newIndividualName,
            null,
            true,
            newIsActive,
            newDisplayName,
            newMainAddress,
            newIdentifiers,
            newRoleIds,
            null,
            newContacts));
    }

    [Fact]
    public void Update_NullContacts_ThrowsInvalidOperationException()
    {
        // Arrange
        var partner = Partner.Reconstitute(
            1L,
            new PersonName("Old", "Name"),
            null,
            true,
            true,
            "Oldie",
            new Address("Second St", "123", "City", "State", "12345", "US"),
            new List<PartnerIdentifier>
            {
                PartnerIdentifier.Create("SSN", "000-00-0000")
            },
            new List<long>() { 1L },
            new List<PartnerBankAccount>(),
            new List<PartnerContact>(),
            PartnersTestData.GetMetadata());

        var newIndividualName = new PersonName("New", "Name");
        string newDisplayName = "New Name";
        Address newMainAddress = new Address("Main St", "123", "City", "State", "12345", "US");
        bool newIsActive = false;
        List<PartnerIdentifier> newIdentifiers = new();
        List<long> newRoleIds = new();
        List<PartnerBankAccount> newBankAccounts = new();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => partner.Update(
            newIndividualName,
            null,
            true,
            newIsActive,
            newDisplayName,
            newMainAddress,
            newIdentifiers,
            newRoleIds,
            newBankAccounts,
            null));
    }

    [Theory]
    [InlineData(true, "John", "Doe", null, "John Doe")]
    [InlineData(true, "Alice", "O'Connor", null, "Alice O'Connor")]
    [InlineData(false, null, null, "Acme Corp", "Acme Corp")]
    [InlineData(false, null, null, "  MegaInc  ", "  MegaInc  ")]
    public void FullName_ReturnsCorrectValue(
        bool isNaturalPerson,
        string? firstName,
        string? lastName,
        string? companyName,
        string expected)
    {
        // Arrange
        var partner = Partner.Reconstitute(
            id: 1,
            individualName: isNaturalPerson ? new PersonName(firstName!, lastName!) : null,
            companyName: companyName,
            isNaturalPerson: isNaturalPerson,
            isActive: true,
            displayName: null,
            hqAddress: null,
            identifiers: new List<PartnerIdentifier>()
            {
                PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
            },
            roleIds: new List<long>() { 1L },
            bankAccounts: Array.Empty<PartnerBankAccount>(),
            contacts: Array.Empty<PartnerContact>(),
            PartnersTestData.GetMetadata());

        //Act & Assert
        partner.FullName.Should().Be(expected);
    }

    [Theory]
    [InlineData(true, "John", "Doe", null, null, "John Doe")]
    [InlineData(true, "Mary", "Smith", null, "Johnny", "Mary Smith (Johnny)")]
    [InlineData(true, "Alice", "O'Connor", null, "Alice in Wonderland", "Alice O'Connor (Alice in Wonderland)")]
    [InlineData(true, "Bob", "Jones", null, "   ", "Bob Jones")]
    [InlineData(true, "Eve", "Brown", null, "", "Eve Brown")]

    [InlineData(false, null, null, "Acme Corp", null, "Acme Corp")]
    [InlineData(false, null, null, "MegaInc", "Trading Division", "MegaInc (Trading Division)")]
    [InlineData(false, null, null, "Globex", "   ", "Globex")]
    public void FullNameWithDisplay_ReturnsCorrectValue(
    bool isNaturalPerson,
    string? firstName,
    string? lastName,
    string? companyName,
    string? displayName,
    string expected)
    {
        var partner = Partner.Reconstitute(
            id: 1L,
            individualName: isNaturalPerson ? new PersonName(firstName!, lastName!) : null,
            companyName: companyName,
            isNaturalPerson: isNaturalPerson,
            isActive: true,
            displayName: displayName,
            hqAddress: null,
            identifiers: new List<PartnerIdentifier>()
            {
                PartnerIdentifier.Create("Abc", "111-11-1111", isPrimary: true)
            },
            roleIds: new List<long>() { 1L },
            bankAccounts: Array.Empty<PartnerBankAccount>(),
            contacts: Array.Empty<PartnerContact>(),
            PartnersTestData.GetMetadata());

        Assert.Equal(expected, partner.FullNameWithDisplay);
    }
}
