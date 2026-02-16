using Ogma.Application.Partners.Dtos;
using Ogma.Application.SharedKernel.Dtos;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.SharedKernel.ValueObjects;
using Ogma.Infrastructure.Persistence.SharedKernel.Extensions;
using Ogma.Infrastructure.Persistence.SharedKernel.ValueObjectRecords;

namespace Ogma.Infrastructure.Persistence.Partners.Extensions;

public static class PartnersMappingExtensions
{
    #region ToDomain

    /// <summary>
    /// Converts a partner role type model to its corresponding domain entity representation.
    /// </summary>
    /// <param name="partnerRoleType">The partner role type model to convert. Cannot be null.</param>
    /// <returns>A domain entity representing the specified partner role type.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="partnerRoleType"/> is null.</exception>
    public static PartnerRoleType ToDomain(this Models.PartnerRoleType partnerRoleType)
    {
        ArgumentNullException.ThrowIfNull(partnerRoleType, nameof(partnerRoleType));
        var domainPartnerRoleType = PartnerRoleType.Reconstitute(
            partnerRoleType.Id,
            partnerRoleType.Name,
            partnerRoleType.Code,
            partnerRoleType.Color
        );
        return domainPartnerRoleType;
    }

    /// <summary>
    /// Converts an <see cref="SharedKernel.ValueObjectRecords.AddressRecord"/> instance to its corresponding domain <see
    /// cref="Address"/> value object.
    /// </summary>
    /// <param name="addressRecord">The address record to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A domain address value object containing the data from the specified address record.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="addressRecord"/> is <see langword="null"/>.</exception>
    public static Address ToDomain(this AddressRecord addressRecord)
    {
        ArgumentNullException.ThrowIfNull(addressRecord, nameof(addressRecord));
        var domainAddress = new Address(
            street: addressRecord.Street,
            number: addressRecord.Number,
            city: addressRecord.City,
            region: addressRecord.Region,
            postalCode: addressRecord.PostalCode,
            countryCode: addressRecord.CountryCode,
            building: addressRecord.Building,
            staircase: addressRecord.Staircase,
            floor: addressRecord.Floor,
            apartment: addressRecord.Apartment
        );
        return domainAddress;
    }

    /// <summary>
    /// Converts a <see cref="SharedKernel.ValueObjectRecords.BankAccountRecord"/> instance to its corresponding domain <see
    /// cref="BankAccount"/> value object.
    /// </summary>
    /// <param name="bankAccountRecord">The bank account record to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="BankAccount"/> instance representing the specified bank account
    /// record.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="bankAccountRecord"/> is <see langword="null"/>.</exception>
    public static BankAccount ToDomain(this BankAccountRecord bankAccountRecord)
    {
        ArgumentNullException.ThrowIfNull(bankAccountRecord, nameof(bankAccountRecord));
        var domainBankAccount = new BankAccount(
            bank: bankAccountRecord.Bank,
            iban: bankAccountRecord.Iban,
            currency: bankAccountRecord.Currency,
            bic: bankAccountRecord.Bic
        );
        return domainBankAccount;
    }

    /// <summary>
    /// Converts a <see cref="Models.PartnerIdentifier"/> instance to its corresponding domain <see
    /// cref="PartnerIdentifier"/> representation.
    /// </summary>
    /// <param name="partnerIdentifier">The source <see cref="Models.PartnerIdentifier"/> to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="PartnerIdentifier"/> instance containing the mapped values from the
    /// specified model.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="partnerIdentifier"/> is <see langword="null"/>.</exception>
    public static PartnerIdentifier ToDomain(this Models.PartnerIdentifier partnerIdentifier)
    {
        ArgumentNullException.ThrowIfNull(partnerIdentifier, nameof(partnerIdentifier));
        var domainPartnerIdentifier = PartnerIdentifier.Reconstitute(
            id: partnerIdentifier.Id,
            type: partnerIdentifier.Type,
            value: partnerIdentifier.Value,
            validityPeriod: partnerIdentifier.ValidityStart.HasValue ? new Period(partnerIdentifier.ValidityStart.Value, partnerIdentifier.ValidityEnd) : null,
            isPrimary: partnerIdentifier.IsPrimary
        );
        return domainPartnerIdentifier;
    }

    /// <summary>
    /// Converts a data model representation of a partner bank account to its corresponding domain entity.
    /// </summary>
    /// <param name="partnerBankAccount">The partner bank account model to convert. Cannot be null.</param>
    /// <returns>A domain entity representing the partner bank account with values mapped from the provided model.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="partnerBankAccount"/> is null.</exception>
    public static PartnerBankAccount ToDomain(this Models.PartnerBankAccount partnerBankAccount)
    {
        ArgumentNullException.ThrowIfNull(partnerBankAccount, nameof(partnerBankAccount));
        var domainPartnerBankAccount = PartnerBankAccount.Reconstitute(
            id: partnerBankAccount.Id,
            bankAccount: partnerBankAccount.BankAccount.ToDomain(),
            isDefault: partnerBankAccount.IsDefault
        );
        return domainPartnerBankAccount;
    }

    /// <summary>
    /// Converts a <see cref="Models.PartnerContact"/> model to its corresponding domain <see
    /// cref="PartnerContact"/> entity.
    /// </summary>
    /// <param name="partnerContact">The partner contact model to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="PartnerContact"/> entity representing the provided model.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="partnerContact"/> is <see langword="null"/>.</exception>
    public static PartnerContact ToDomain(this Models.PartnerContact partnerContact)
    {
        ArgumentNullException.ThrowIfNull(partnerContact, nameof(partnerContact));
        var domainPartnerContact = PartnerContact.Reconstitute(
            id: partnerContact.Id,
            name: new PersonName(partnerContact.FirstName, partnerContact.LastName),
            email: new Email(partnerContact.Email),
            phone: partnerContact.Phone,
            mobile: partnerContact.Mobile,
            title: partnerContact.Title,
            jobTitle: partnerContact.JobTitle,
            isPrimary: partnerContact.IsPrimary
        );
        return domainPartnerContact;
    }

    /// <summary>
    /// Converts a data model partner to its corresponding domain entity representation.
    /// </summary>
    /// <param name="partner"></param>
    /// <returns></returns>
    public static Partner ToDomain(this Models.Partner partner)
    {
        ArgumentNullException.ThrowIfNull(partner, nameof(partner));
        var domainPartner = Partner.Reconstitute(
            id: partner.Id,
            individualName: partner.IsNaturalPerson && partner.IndividualFirstName != null && partner.IndividualLastName != null
                ? new PersonName(partner.IndividualFirstName, partner.IndividualLastName)
                : null,
            companyName: partner.IsNaturalPerson ? null : partner.CompanyName,
            isNaturalPerson: partner.IsNaturalPerson,
            isActive: partner.IsActive,
            displayName: partner.DisplayName,
            hqAddress: partner.HQAddress?.ToDomain(),
            identifiers: partner.Identifiers?.Select(i => i.ToDomain()).ToList() ?? new List<PartnerIdentifier>(),
            roleIds: partner.Roles?.Select(r => r.Id).ToList() ?? new List<long>(),
            bankAccounts: partner.BankAccounts?.Select(b => b.ToDomain()).ToList() ?? new List<PartnerBankAccount>(),
            contacts: partner.Contacts?.Select(c => c.ToDomain()).ToList() ?? new List<PartnerContact>()
        );
        return domainPartner;
    }

    #endregion

    #region ToModel

    /// <summary>
    /// Converts a domain partner role type entity to its corresponding model representation.
    /// </summary>
    /// <param name="partnerRoleType">The domain partner role type entity to convert. Cannot be null.</param>
    /// <returns>A <see cref="Models.PartnerRoleType"/> instance containing the mapped values from the specified domain entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="partnerRoleType"/> is null.</exception>
    public static Models.PartnerRoleType ToModel(this PartnerRoleType partnerRoleType)
    {
        ArgumentNullException.ThrowIfNull(partnerRoleType, nameof(partnerRoleType));
        var modelPartnerRoleType = new Models.PartnerRoleType
        {
            Id = partnerRoleType.Id,
            Name = partnerRoleType.Name,
            Code = partnerRoleType.Code,
            Color = partnerRoleType.Color
        };
        return modelPartnerRoleType;
    }

    /// <summary>
    /// Converts an <see cref="Address"/> instance to its corresponding <see
    /// cref="SharedKernel.ValueObjectRecords.AddressRecord"/> model representation.
    /// </summary>
    /// <param name="address">The address value object to convert. Cannot be null.</param>
    /// <returns>An <see cref="SharedKernel.ValueObjectRecords.AddressRecord"/> containing the data from the specified address.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="address"/> is null.</exception>
    public static AddressRecord ToModel(this Address address)
    {
        ArgumentNullException.ThrowIfNull(address, nameof(address));
        var addressRecord = new AddressRecord(
            Street: address.Street,
            Number: address.Number,
            City: address.City,
            Region: address.Region,
            PostalCode: address.PostalCode,
            CountryCode: address.CountryCode,
            Building: address.Building,
            Staircase: address.Staircase,
            Floor: address.Floor,
            Apartment: address.Apartment
        );
        return addressRecord;
    }

    /// <summary>
    /// Converts a <see cref="BankAccount"/> instance to its corresponding <see
    /// cref="SharedKernel.ValueObjectRecords.BankAccountRecord"/> model representation.
    /// </summary>
    /// <param name="bankAccount">The bank account value object to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="SharedKernel.ValueObjectRecords.BankAccountRecord"/> that represents the specified bank account.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="bankAccount"/> is <see langword="null"/>.</exception>
    public static BankAccountRecord ToModel(this BankAccount bankAccount)
    {
        ArgumentNullException.ThrowIfNull(bankAccount, nameof(bankAccount));
        var bankAccountRecord = new BankAccountRecord(
            Bank: bankAccount.Bank,
            Iban: bankAccount.Iban,
            Currency: bankAccount.Currency,
            Bic: bankAccount.Bic
        );
        return bankAccountRecord;
    }

    /// <summary>
    /// Converts a domain partner identifier entity to its corresponding model representation.
    /// </summary>
    /// <param name="partnerIdentifier">The domain partner identifier to convert. Cannot be null.</param>
    /// <returns>A <see cref="Models.PartnerIdentifier"/> instance containing the mapped values from the specified domain entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="partnerIdentifier"/> is null.</exception>
    public static Models.PartnerIdentifier ToModel(this PartnerIdentifier partnerIdentifier)
    {
        ArgumentNullException.ThrowIfNull(partnerIdentifier, nameof(partnerIdentifier));
        var modelPartnerIdentifier = new Models.PartnerIdentifier
        {
            Id = partnerIdentifier.Id,
            Type = partnerIdentifier.Type,
            Value = partnerIdentifier.Value,
            ValidityStart = partnerIdentifier.ValidityPeriod! != null! ? partnerIdentifier.ValidityPeriod.Start : null,
            ValidityEnd = partnerIdentifier.ValidityPeriod! != null! ? partnerIdentifier.ValidityPeriod.End : null,
            IsPrimary = partnerIdentifier.IsPrimary
        };
        return modelPartnerIdentifier;
    }

    /// <summary>
    /// Converts a domain partner bank account entity to its corresponding model representation.
    /// </summary>
    /// <param name="partnerBankAccount">The domain partner bank account entity to convert. Cannot be null.</param>
    /// <returns>A <see cref="Models.PartnerBankAccount"/> instance containing the mapped data from the specified domain entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="partnerBankAccount"/> is null.</exception>
    public static Models.PartnerBankAccount ToModel(this PartnerBankAccount partnerBankAccount)
    {
        ArgumentNullException.ThrowIfNull(partnerBankAccount, nameof(partnerBankAccount));
        var modelPartnerBankAccount = new Models.PartnerBankAccount
        {
            Id = partnerBankAccount.Id,
            BankAccount = partnerBankAccount.BankAccount.ToModel(),
            IsDefault = partnerBankAccount.IsDefault
        };
        return modelPartnerBankAccount;
    }

    /// <summary>
    /// Converts a domain partner contact entity to its corresponding model representation.
    /// </summary>
    /// <param name="partnerContact">The domain partner contact entity to convert. Cannot be null.</param>
    /// <returns>A <see cref="Models.PartnerContact"/> instance containing the mapped data from the specified domain entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="partnerContact"/> is null.</exception>
    public static Models.PartnerContact ToModel(this PartnerContact partnerContact)
    {
        ArgumentNullException.ThrowIfNull(partnerContact, nameof(partnerContact));
        var modelPartnerContact = new Models.PartnerContact
        {
            Id = partnerContact.Id,
            FirstName = partnerContact.Name.FirstName,
            LastName = partnerContact.Name.LastName,
            Email = partnerContact.Email?.Value,
            Phone = partnerContact.Phone,
            Mobile = partnerContact.Mobile,
            Title = partnerContact.Title,
            JobTitle = partnerContact.JobTitle,
            IsPrimary = partnerContact.IsPrimary
        };
        return modelPartnerContact;
    }

    /// <summary>
    /// Converts a domain partner entity to its corresponding model representation.
    /// </summary>
    /// <param name="partner"></param>
    /// <returns></returns>
    public static Models.Partner ToModel(this Partner partner)
    {
        ArgumentNullException.ThrowIfNull(partner, nameof(partner));
        var modelPartner = new Models.Partner
        {
            Id = partner.Id,
            IndividualFirstName = partner.IsNaturalPerson && partner.IndividualName != null ? partner.IndividualName.FirstName : null,
            IndividualLastName = partner.IsNaturalPerson && partner.IndividualName != null ? partner.IndividualName.LastName : null,
            CompanyName = partner.IsNaturalPerson ? null : partner.CompanyName,
            IsNaturalPerson = partner.IsNaturalPerson,
            IsActive = partner.IsActive,
            DisplayName = partner.DisplayName,
            HQAddress = partner.HQAddress?.ToModel(),
            Identifiers = partner.Identifiers?.Select(i => i.ToModel()).ToList() ?? new List<Models.PartnerIdentifier>(),
            BankAccounts = partner.BankAccounts?.Select(b => b.ToModel()).ToList() ?? new List<Models.PartnerBankAccount>(),
            Contacts = partner.Contacts?.Select(c => c.ToModel()).ToList() ?? new List<Models.PartnerContact>()
        };
        return modelPartner;
    }

    #endregion

    #region ToDto

    /// <summary>
    /// Converts a domain partner role type entity to its corresponding data transfer object (DTO) representation.
    /// </summary>
    /// <param name="partnerRoleType"></param>
    /// <returns></returns>
    public static PartnerRoleTypeDto ToDto(this Models.PartnerRoleType partnerRoleType)
    {
        ArgumentNullException.ThrowIfNull(partnerRoleType, nameof(partnerRoleType));
        var dto = new PartnerRoleTypeDto(
            Id: partnerRoleType.Id,
            Code: partnerRoleType.Code,
            Name: partnerRoleType.Name,
            Color: partnerRoleType.Color
        );
        return dto;
    }

    /// <summary>
    /// Converts a PartnerIdentifier model to its corresponding data transfer object (DTO) representation.
    /// </summary>
    /// <param name="partnerIdentifier"></param>
    /// <returns></returns>
    public static PartnerIdentifierDto ToDto(this Models.PartnerIdentifier partnerIdentifier)
    {
        ArgumentNullException.ThrowIfNull(partnerIdentifier, nameof(partnerIdentifier));
        var dto = new PartnerIdentifierDto(
            Id: partnerIdentifier.Id,
            Type: partnerIdentifier.Type,
            Value: partnerIdentifier.Value,
            ValidityPeriod: partnerIdentifier.ValidityStart.HasValue ? new PeriodDto(partnerIdentifier.ValidityStart.Value, partnerIdentifier.ValidityEnd) : null,
            IsPrimary: partnerIdentifier.IsPrimary
        );
        return dto;
    }

    /// <summary>
    /// Converts a PartnerBankAccount model to its corresponding data transfer object (DTO) representation.
    /// </summary>
    /// <param name="partnerBankAccount"></param>
    /// <returns></returns>
    public static PartnerBankAccountDto ToDto(this Models.PartnerBankAccount partnerBankAccount)
    {
        ArgumentNullException.ThrowIfNull(partnerBankAccount, nameof(partnerBankAccount));
        var dto = new PartnerBankAccountDto(
            Id: partnerBankAccount.Id,
            BankAccount: partnerBankAccount.BankAccount.ToDto(),
            IsDefault: partnerBankAccount.IsDefault
        );
        return dto;
    }

    /// <summary>
    /// Converts a PartnerContact model to its corresponding data transfer object (DTO) representation.
    /// </summary>
    /// <param name="partnerContact"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static PartnerContactDto ToDto(this Models.PartnerContact partnerContact)
    {
        if (partnerContact == null)
        {
            throw new ArgumentNullException(nameof(partnerContact));
        }
        var dto = new PartnerContactDto(
            Id: partnerContact.Id,
            Name: new PersonNameDto(partnerContact.FirstName, partnerContact.LastName),
            Email: partnerContact.Email,
            Phone: partnerContact.Phone,
            Mobile: partnerContact.Mobile,
            Title: partnerContact.Title,
            JobTitle: partnerContact.JobTitle,
            IsPrimary: partnerContact.IsPrimary
        );
        return dto;
    }

    /// <summary>
    /// Converts a Partner model to its corresponding data transfer object (DTO) representation.
    /// </summary>
    /// <param name="partner"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static PartnerDto ToDto(this Models.Partner partner)
    {
        if (partner == null)
        {
            throw new ArgumentNullException(nameof(partner));
        }
        var dto = new PartnerDto(
            Id: partner.Id,
            IndividualName: partner.IsNaturalPerson && partner.IndividualFirstName != null && partner.IndividualLastName != null
                ? new PersonNameDto(partner.IndividualFirstName, partner.IndividualLastName)
                : null,
            CompanyName: partner.IsNaturalPerson ? null : partner.CompanyName,
            IsNaturalPerson: partner.IsNaturalPerson,
            IsActive: partner.IsActive,
            DisplayName: partner.DisplayName,
            HQAddress: partner.HQAddress?.ToDto(),
            Identifiers: partner.Identifiers?.Select(i => i.ToDto()).ToList() ?? new List<PartnerIdentifierDto>(),
            Roles: partner.Roles?.Select(r => r.ToDto()).ToList() ?? new List<PartnerRoleTypeDto>(),
            BankAccounts: partner.BankAccounts?.Select(b => b.ToDto()).ToList() ?? new List<PartnerBankAccountDto>(),
            Contacts: partner.Contacts?.Select(c => c.ToDto()).ToList() ?? new List<PartnerContactDto>()
        );
        return dto;
    }

    #endregion
}
