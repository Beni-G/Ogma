using Bogus;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.UnitTests.SharedKernel.ValueObjects.Helpers;

public static class SharedFaker
{
    private static readonly Faker Faker = new("de");

    public static PersonName CreatePersonName(string? firstName = null, string? lastName = null) =>
        new(firstName ?? Faker.Name.FirstName(), lastName ?? Faker.Name.LastName());

    public static Address CreateAddress() => new(
        Faker.Address.StreetName(),
        Faker.Address.BuildingNumber(),
        Faker.Address.City(),
        Faker.Address.State(),
        Faker.Address.ZipCode(),
        Faker.Address.CountryCode(),
        $"{new Faker().PickRandom("Block", "Building", "Gebäude")} {new Faker().Address.SecondaryAddress()}",
        Faker.Random.Bool(0.1f) ? Faker.Random.Replace("#") : null,
        Faker.Random.Bool(0.1f) ? Faker.Random.Number(1, 10).ToString() : null,
        Faker.Random.Bool(0.1f) ? Faker.Random.Number(101, 999).ToString() : null
    );

    public static BankAccount CreateBankAccountDto(string countryCode = "DE")
    {
        var f = new Faker("de");
        return new BankAccount(
            bank: f.Company.CompanyName() + " Bank",
            iban: f.Finance.Iban(countryCode: countryCode),
            currency: "EUR",
            bic: f.Finance.Bic()
        );
    }

    public static Period CreatePeriod()
    {
        var f = new Faker();
        var start = f.Date.Past(1);
        return new Period(start, start.AddYears(2));
    }
}
