using Bogus;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.UnitTests.Partners.Helpers;

internal static class PartnersTestData
{
    private static readonly Faker _faker = new();

    public static long NextId() => Random.Shared.NextInt64(1, long.MaxValue);

    public static EntityMetadata GetMetadata() => new EntityMetadata(DateTime.UtcNow, DateTime.UtcNow, 1);

    public static class Identifiers
    {
        public static PartnerIdentifierInput Random(long? id, string identifierType, bool isPrimary = false)
        {
            return new PartnerIdentifierInput(
                Id: id ?? _faker.Random.Long(1, 10000),
                Type: identifierType,
                Value: _faker.Random.Replace("##-#######-X"),
                ValidityPeriod: _faker.Random.Bool(0.7f)
                    ? new Period(
                        _faker.Date.Recent(10).ToUniversalTime(),
                        _faker.Date.Future(3).ToUniversalTime())
                    : null,
                IsPrimary: isPrimary
            );
        }

        public static PartnerIdentifierInput From(PartnerIdentifier existingIdentifier)
        {
            return new PartnerIdentifierInput(
                Id: existingIdentifier.Id,
                Type: existingIdentifier.Type,
                Value: existingIdentifier.Value,
                ValidityPeriod: existingIdentifier.ValidityPeriod,
                IsPrimary: existingIdentifier.IsPrimary
            );
        }
    }
    
    public static class BankAccounts
    {
        public static PartnerBankAccountInput Random(long? id) =>
            new(
                Id: id ?? _faker.Random.Long(1, 10000),
                BankAccount: new BankAccount(_faker.Company.CompanyName(),
                    _faker.Finance.Iban(), _faker.Finance.Currency().Code),
                IsDefault: _faker.Random.Bool(0.2f)
            );

        public static PartnerBankAccountInput From(PartnerBankAccount existingBankAccount) =>
            new(
                Id: existingBankAccount.Id,
                BankAccount: existingBankAccount.BankAccount,
                IsDefault: existingBankAccount.IsDefault
            );
    }

    public static class Contacts
    {
        public static PartnerContactInput Random(long? id)
        {
            var titles = new[] { "Mr.", "Ms.", "Dr.", "Prof." };
            var jobTitles = new[] { "Accountant", "Procurement Manager", "CEO", "Logistics Specialist", "Legal Counsel" };

            var firstName = _faker.Name.FirstName();
            var lastName = _faker.Name.LastName();
            var domainName = "ogma-test.de";

            return new PartnerContactInput(
                Id: id ?? _faker.Random.Long(1, 10000),
                Name: new PersonName(firstName, lastName),
                Email: _faker.Random.Bool(0.9f)
                    ? $"{firstName.ToLower()}.{lastName.ToLower()}@{domainName}"
                    : null,
                Phone: _faker.Random.Bool(0.7f) ? _faker.Phone.PhoneNumber("+49 (0) ### #######") : null,
                Mobile: _faker.Random.Bool(0.6f) ? _faker.Phone.PhoneNumber("+49 17# #######") : null,
                Title: _faker.Random.Bool(0.4f) ? _faker.PickRandom(titles) : null,
                JobTitle: _faker.Random.Bool(0.8f) ? _faker.PickRandom(jobTitles) : null,
                IsPrimary: _faker.Random.Bool(0.15f)
            );
        }

        public static PartnerContactInput From(PartnerContact existingContact)
        {
            return new PartnerContactInput(
                Id: existingContact.Id,
                Name: existingContact.Name,
                Email: existingContact.Email is not null ? existingContact.Email.Value : null,
                Phone: existingContact.Phone,
                Mobile: existingContact.Mobile,
                Title: existingContact.Title,
                JobTitle: existingContact.JobTitle,
                IsPrimary: existingContact.IsPrimary
            );
        }
    }

}
