using Bogus;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.UnitTests.SharedKernel.ValueObjects.Helpers;

namespace Ogma.Application.UnitTests.Partners.Helpers;

public class PartnerBuilder
{
    private readonly Faker _faker = new("de");
    private bool _isNaturalPerson = true;
    private string? _customCompanyName;
    private List<long> _roleIds = new();
    private List<PartnerIdentifier> _identifiers = new();

    public PartnerBuilder AsLegalEntity(string? companyName = null)
    {
        _isNaturalPerson = false;
        _customCompanyName = companyName;
        return this;
    }

    public PartnerBuilder AsNaturalPerson()
    {
        _isNaturalPerson = true;
        return this;
    }

    public PartnerBuilder WithRoles(params long[] ids)
    {
        _roleIds = ids.ToList();
        return this;
    }

    public PartnerBuilder WithIdentifier(string type, string value)
    {
        _identifiers.Add(PartnerIdentifier.Reconstitute(
            id: _faker.Random.Long(1),
            type: type,
            value: value,
            validityPeriod: SharedFaker.CreatePeriod(),
            isPrimary: !_identifiers.Any()
        ));
        return this;
    }

    public Partner Build()
    {
        var individualName = _isNaturalPerson
            ? SharedFaker.CreatePersonName()
            : null;

        var companyName = !_isNaturalPerson
            ? (_customCompanyName ?? _faker.Company.CompanyName())
            : null;

        var displayName = _isNaturalPerson
            ? $"{individualName!.FirstName} {individualName.LastName}"
            : companyName!;

        if (!_roleIds.Any())
        {
            _roleIds.Add(_faker.Random.Long(1, 100));
        }

        if (!_identifiers.Any())
        {
            _identifiers.Add(PartnerIdentifier.Reconstitute(
                id: _faker.Random.Long(1),
                type: "VAT_ID",
                value: _faker.Random.AlphaNumeric(8),
                validityPeriod: SharedFaker.CreatePeriod(),
                isPrimary: true
            ));
        }

        return Partner.Reconstitute(
            id: _faker.Random.Long(1),
            individualName: individualName,
            companyName: companyName,
            isNaturalPerson: _isNaturalPerson,
            isActive: true,
            displayName: displayName,
            hqAddress: SharedFaker.CreateAddress(),
            identifiers: _identifiers,
            roleIds: _roleIds,
            bankAccounts: new List<PartnerBankAccount>(),
            contacts: new List<PartnerContact>()
        );
    }
}

