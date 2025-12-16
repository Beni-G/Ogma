using Ogma.Domain.Partners.Entities;
using Ogma.Domain.SharedKernel.ValueObjects;

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
        if (partnerRoleType == null)
        {
            throw new ArgumentNullException(nameof(partnerRoleType));
        }
        var domainPartnerRoleType = PartnerRoleType.Reconstitute(
            partnerRoleType.Id,
            partnerRoleType.Name,
            partnerRoleType.Code,
            partnerRoleType.Color
        );
        return domainPartnerRoleType;
    }

    /// <summary>
    /// Converts an <see cref="ValueObjectRecords.AddressRecord"/> instance to its corresponding domain <see
    /// cref="Address"/> value object.
    /// </summary>
    /// <param name="addressRecord">The address record to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A domain address value object containing the data from the specified address record.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="addressRecord"/> is <see langword="null"/>.</exception>
    public static Address ToDomain(this ValueObjectRecords.AddressRecord addressRecord)
    {
        if (addressRecord == null)
        {
            throw new ArgumentNullException(nameof(addressRecord));
        }
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
    /// Converts a <see cref="ValueObjectRecords.BankAccountRecord"/> instance to its corresponding domain <see
    /// cref="BankAccount"/> value object.
    /// </summary>
    /// <param name="bankAccountRecord">The bank account record to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="BankAccount"/> instance representing the specified bank account
    /// record.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="bankAccountRecord"/> is <see langword="null"/>.</exception>
    public static BankAccount ToDomain(this ValueObjectRecords.BankAccountRecord bankAccountRecord)
    {
        if (bankAccountRecord == null)
        {
            throw new ArgumentNullException(nameof(bankAccountRecord));
        }
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
        if (partnerIdentifier == null)
        {
            throw new ArgumentNullException(nameof(partnerIdentifier));
        }
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
        if (partnerBankAccount == null)
        {
            throw new ArgumentNullException(nameof(partnerBankAccount));
        }
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
        if (partnerContact == null)
        {
            throw new ArgumentNullException(nameof(partnerContact));
        }
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
    /// <remarks>Use this method to map a partner from the data model layer to the domain layer, preserving
    /// all relevant properties and relationships.</remarks>
    /// <param name="partner">The partner model to convert. Cannot be null.</param>
    /// <returns>A domain partner entity that represents the specified model partner.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="partner"/> is null.</exception>
    public static Partner ToDomain(this Models.Partner partner)
    {
        if (partner == null)
        {
            throw new ArgumentNullException(nameof(partner));
        }
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
            roles: partner.Roles?.Select(r => r.ToDomain()).ToList() ?? new List<PartnerRoleType>(),
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
        if (partnerRoleType == null)
        {
            throw new ArgumentNullException(nameof(partnerRoleType));
        }
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
    /// cref="ValueObjectRecords.AddressRecord"/> model representation.
    /// </summary>
    /// <param name="address">The address value object to convert. Cannot be null.</param>
    /// <returns>An <see cref="ValueObjectRecords.AddressRecord"/> containing the data from the specified address.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="address"/> is null.</exception>
    public static ValueObjectRecords.AddressRecord ToModel(this Address address)
    {
        if (address == null)
        {
            throw new ArgumentNullException(nameof(address));
        }
        var addressRecord = new ValueObjectRecords.AddressRecord(
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
    /// cref="ValueObjectRecords.BankAccountRecord"/> model representation.
    /// </summary>
    /// <param name="bankAccount">The bank account value object to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="ValueObjectRecords.BankAccountRecord"/> that represents the specified bank account.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="bankAccount"/> is <see langword="null"/>.</exception>
    public static ValueObjectRecords.BankAccountRecord ToModel(this BankAccount bankAccount)
    {
        if (bankAccount == null)
        {
            throw new ArgumentNullException(nameof(bankAccount));
        }
        var bankAccountRecord = new ValueObjectRecords.BankAccountRecord(
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
        if (partnerIdentifier == null)
        {
            throw new ArgumentNullException(nameof(partnerIdentifier));
        }
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
        if (partnerBankAccount == null)
        {
            throw new ArgumentNullException(nameof(partnerBankAccount));
        }
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
        if (partnerContact == null)
        {
            throw new ArgumentNullException(nameof(partnerContact));
        }
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
    /// <remarks>Use this extension method to map domain partner entities to model objects for presentation or
    /// API responses. All relevant properties are mapped; collections are converted to their model equivalents. If a
    /// property in the domain entity is null, the corresponding model property will also be null or an empty collection
    /// as appropriate.</remarks>
    /// <param name="partner">The domain partner entity to convert. Cannot be null.</param>
    /// <returns>A <see cref="Models.Partner"/> instance containing the mapped data from the specified domain partner entity.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="partner"/> is null.</exception>
    public static Models.Partner ToModel(this Partner partner)
    {
        if (partner == null)
        {
            throw new ArgumentNullException(nameof(partner));
        }
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
            Roles = partner.Roles?.Select(r => r.ToModel()).ToList() ?? new List<Models.PartnerRoleType>(),
            BankAccounts = partner.BankAccounts?.Select(b => b.ToModel()).ToList() ?? new List<Models.PartnerBankAccount>(),
            Contacts = partner.Contacts?.Select(c => c.ToModel()).ToList() ?? new List<Models.PartnerContact>()
        };
        return modelPartner;
    }

    #endregion
}
