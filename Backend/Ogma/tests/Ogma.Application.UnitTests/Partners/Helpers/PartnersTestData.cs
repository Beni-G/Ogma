using Ogma.Application.Partners.Commands;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.SharedKernel.Dtos;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.UnitTests.Partners.Helpers;

internal static class PartnersTestData
{
    public static long NextId() => Random.Shared.NextInt64(1, long.MaxValue);

    public static string CreateName() => $"Name_{Guid.NewGuid().ToString()[..8]}";

    public static string CreateCode() => $"Code_{Guid.NewGuid().ToString()[..8]}";

    public static string CreateColor() => $"Color_{Guid.NewGuid().ToString()[..6]}";

    public static PeriodDto CreatePeriodDto() => new(
        Start: DateTime.UtcNow.AddDays(-30),
        End: DateTime.UtcNow.AddDays(30)
    );

    public static BankAccountDto CreateBankAccountDto() => new(
        Bank: $"Bank_{Guid.NewGuid().ToString()[..8]}",
        Iban: "DE75512108001245126199",
        Currency: "EUR",
        Bic: "Abc"
    );

    public static PersonNameDto CreatePersonNameDto() => new(
        FirstName: $"FirstName_{Guid.NewGuid().ToString()[..8]}",
        LastName: $"LastName_{Guid.NewGuid().ToString()[..8]}"
    );

    public static AddressDto CreateAddressDto() => new(
        Street: $"Street_{Guid.NewGuid().ToString()[..5]}",
        Number: $"Number_{Guid.NewGuid().ToString()[..2]}",
        City: $"City_{Guid.NewGuid().ToString()[..5]}",
        Region: $"Region_{Guid.NewGuid().ToString()[..5]}",
        PostalCode: $"PostalCode_{Guid.NewGuid().ToString()[..5]}",
        CountryCode: "DE",
        Building: $"Building_{Guid.NewGuid().ToString()[..5]}",
        Staircase: $"Staircase_{Guid.NewGuid().ToString()[..5]}",
        Floor: $"Floor_{Guid.NewGuid().ToString()[..2]}",
        Apartment: $"Apartment_{Guid.NewGuid().ToString()[..2]}"
    );

    public static string CreateEmail() => $"Email_{Guid.NewGuid().ToString()[..5]}" +
        $"@{Guid.NewGuid().ToString()[..5]}" +
        $".{Guid.NewGuid().ToString()[..3]}";

    public static PartnerContactDto CreatePartnerContactDto() => new(
        Id: NextId(),
        Name: CreatePersonNameDto(),
        Email: CreateEmail(),
        Phone: "+12 234 56789",
        Mobile: "+98 765 4321",
        Title: "Sir",
        JobTitle: "GM",
        IsPrimary: true
    );

    public static PartnerBankAccountDto CreatePartnerBankAccountDto() => new(
        Id: NextId(),
        BankAccount: CreateBankAccountDto(),
        IsDefault: Random.Shared.Next(0, 2) == 1
    );

    public static PartnerIdentifierDto CreatePartnerIdentifierDto(bool isPrimary = true) => new(
        Id: NextId(),
        Type: $"Type_{Guid.NewGuid().ToString()[..8]}",
        Value: $"Value_{Guid.NewGuid().ToString()[..8]}",
        ValidityPeriod: CreatePeriodDto(),
        IsPrimary: isPrimary
    );

    public static CreatePartnerRoleTypeCommand GenerateCreatePartnerRoleTypeCommand() => new(
        Code: CreateCode(),
        Name: CreateName(),
        Color: CreateColor()
    );

    public static PartnerRoleTypeDto CreatePartnerRoleTypeDto() => new(
        Id: NextId(),
        Code: CreateCode(),
        Name: CreateName(),
        Color: CreateColor()
    );

    public static PartnerDto CreateNaturalPersonPartnerDto() => new(
        Id: NextId(),
        IndividualName: CreatePersonNameDto(),
        CompanyName: null,
        IsNaturalPerson: true,
        IsActive: Random.Shared.Next(0, 2) == 1,
        DisplayName: $"DisplayName_{Guid.NewGuid().ToString()[..8]}",
        HQAddress: CreateAddressDto(),
        Roles: new List<PartnerRoleTypeDto> { CreatePartnerRoleTypeDto(), CreatePartnerRoleTypeDto() },
        Identifiers: new List<PartnerIdentifierDto> { CreatePartnerIdentifierDto(), CreatePartnerIdentifierDto(false) },
        BankAccounts: new List<PartnerBankAccountDto> { CreatePartnerBankAccountDto(), CreatePartnerBankAccountDto() },
        Contacts: new List<PartnerContactDto> { CreatePartnerContactDto(), CreatePartnerContactDto() }
    );

    public static PartnerDto CreateLegalEntityPartnerDto() => new(
        Id: NextId(),
        IndividualName: null,
        CompanyName: $"Company_{Guid.NewGuid().ToString()[..8]}",
        IsNaturalPerson: false,
        IsActive: Random.Shared.Next(0, 2) == 1,
        DisplayName: $"DisplayName_{Guid.NewGuid().ToString()[..8]}",
        HQAddress: CreateAddressDto(),
        Roles: new List<PartnerRoleTypeDto> { CreatePartnerRoleTypeDto(), CreatePartnerRoleTypeDto() },
        Identifiers: new List<PartnerIdentifierDto> { CreatePartnerIdentifierDto(), CreatePartnerIdentifierDto(false) },
        BankAccounts: new List<PartnerBankAccountDto> { CreatePartnerBankAccountDto(), CreatePartnerBankAccountDto() },
        Contacts: new List<PartnerContactDto> { CreatePartnerContactDto(), CreatePartnerContactDto() }
    );

    public static CreatePartnerIdentifierDto GenerateCreatePartnerIdentifierDto() => new(
        Type: $"Type_{Guid.NewGuid().ToString()[..8]}",
        Value: $"Value_{Guid.NewGuid().ToString()[..8]}",
        ValidityPeriod: CreatePeriodDto(),
        IsPrimary: true
    );

    public static CreatePartnerCommand GenerateCreatePartnerCommand(bool isNaturalPerson = false)
    {
        if (isNaturalPerson)
        {
            return new(CreatePersonNameDto(), null, isNaturalPerson, CreateAddressDto(), GenerateCreatePartnerIdentifierDto(), NextId());
        }
        else
        {
            return new(null, CreateName(), isNaturalPerson, CreateAddressDto(), GenerateCreatePartnerIdentifierDto(), NextId());
        }
    }

    public static PersonName CreatePersonName() => new($"FirstName_{Guid.NewGuid().ToString()[..8]}", $"LastName_{Guid.NewGuid().ToString()[..8]}");

    public static string CreateCompanyName() => $"CompanyName_{Guid.NewGuid().ToString()[..8]}";

    public static Address CreateAddress() => new(
        street: $"Street_{Guid.NewGuid().ToString()[..5]}",
        number: $"Number_{Guid.NewGuid().ToString()[..2]}",
        city: $"City_{Guid.NewGuid().ToString()[..5]}",
        region: $"Region_{Guid.NewGuid().ToString()[..5]}",
        postalCode: $"PostalCode_{Guid.NewGuid().ToString()[..5]}",
        countryCode: "DE",
        building: $"Building_{Guid.NewGuid().ToString()[..5]}",
        staircase: $"Staircase_{Guid.NewGuid().ToString()[..5]}",
        floor: $"Floor_{Guid.NewGuid().ToString()[..2]}",
        apartment: $"Apartment_{Guid.NewGuid().ToString()[..2]}"
    );

    public static Period CreatePeriod() => new(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddDays(30));

    public static PartnerIdentifier CreatePartnerIdentifier()
    {
        return PartnerIdentifier.Reconstitute(
            id: NextId(),
            type: $"Type_{Guid.NewGuid().ToString()[..8]}",
            value: $"Value_{Guid.NewGuid().ToString()[..8]}",
            metadata: GetMetadata(),
            validityPeriod: CreatePeriod(),
            isPrimary: true);
    }

    public static BankAccount CreateBankAccount() => new(
        bank: $"Bank_{Guid.NewGuid().ToString()[..8]}",
        iban: "DE75512108001245126199",
        currency: "EUR",
        bic: "Abc"
    );

    public static PartnerBankAccount CreatePartnerBankAccount() => PartnerBankAccount.Reconstitute(id: NextId(), bankAccount: CreateBankAccount(), metadata: GetMetadata(), isDefault: Random.Shared.Next(0, 2) == 1);

    public static PartnerContact CreatePartnerContact() => PartnerContact.Reconstitute(
        id: NextId(),
        name: CreatePersonName(),
        metadata: GetMetadata(),
        email: new Email(CreateEmail()),
        phone: "+12 234 56789",
        mobile: "+98 765 4321",
        title: "Sir",
        jobTitle: "GM",
        isPrimary: true
    );
    public static Partner CreatePartner(bool isNaturalPerson, List<long> roleIds)
    {
        return Partner.Reconstitute(
            id: NextId(),
            individualName: isNaturalPerson ? CreatePersonName() : null,
            companyName: !isNaturalPerson ? CreateCompanyName() : null,
            isNaturalPerson: isNaturalPerson,
            isActive: true,
            displayName: $"DisplayName_{Guid.NewGuid().ToString()[..8]}",
            hqAddress: CreateAddress(),
            identifiers: new List<PartnerIdentifier>
                {
                    CreatePartnerIdentifier(),
                    CreatePartnerIdentifier()
                },
            roleIds: roleIds,
            bankAccounts: new List<PartnerBankAccount>
                {
                    CreatePartnerBankAccount(),
                    CreatePartnerBankAccount()
                },
            contacts: new List<PartnerContact>
                {
                    CreatePartnerContact(),
                    CreatePartnerContact()
                },
            metadata: GetMetadata()
            );
    }

    public static EntityMetadata GetMetadata() => new EntityMetadata(DateTime.UtcNow, DateTime.UtcNow, 1);

}
