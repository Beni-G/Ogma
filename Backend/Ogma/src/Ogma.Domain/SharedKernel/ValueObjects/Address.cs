using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Domain.SharedKernel.ValueObjects;

public class Address : ValueObject
{
    public string Street { get; }
    public string Number { get; }
    public string City { get; }
    public string Region { get; }
    public string PostalCode { get; }
    public string CountryCode { get; }
    public string? Building { get; }
    public string? Staircase { get; }
    public string? Floor { get; }
    public string? Apartment { get; }

    public Address(
    string street,
    string number,
    string city,
    string region,
    string postalCode,
    string countryCode,
    string? building = null,
    string? staircase = null,
    string? floor = null,
    string? apartment = null)
    {
        Street = ValidateRequired(street, nameof(street));
        Number = ValidateRequired(number, nameof(number));
        City = ValidateRequired(city, nameof(city));
        Region = ValidateRequired(region, nameof(region));
        PostalCode = ValidateRequired(postalCode, nameof(postalCode));

        if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2 || !countryCode.All(char.IsLetter))
        {
            throw new ArgumentException("Country code must be a valid 2-letter ISO code (e.g. 'US', 'DE').", nameof(countryCode));
        }

        CountryCode = countryCode.ToUpperInvariant();

        Building = building?.Trim() ?? null;  
        Staircase = staircase?.Trim() ?? null;
        Floor = floor?.Trim() ?? null;
        Apartment = apartment?.Trim() ?? null;
    }

    private static string ValidateRequired(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"Address {paramName} is required and cannot be empty or whitespace.", paramName);
        }

        return value.Trim();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return Number;
        yield return Building;
        yield return Staircase;
        yield return Floor;
        yield return Apartment;

        yield return City;
        yield return Region;
        yield return PostalCode;
        yield return CountryCode;
    }

    public override string ToString()
    {
        var addressLines = new List<string>();
        var line1 = Street + " " + Number;
        if (!string.IsNullOrWhiteSpace(Building))
        {
            line1 += ", Building " + Building;
        }

        if (!string.IsNullOrWhiteSpace(Staircase))
        {
            line1 += ", Staircase " + Staircase;
        }

        if (!string.IsNullOrWhiteSpace(Floor))
        {
            line1 += ", Floor " + Floor;
        }

        if (!string.IsNullOrWhiteSpace(Apartment))
        {
            line1 += ", Apt " + Apartment;
        }

        addressLines.Add(line1);
        addressLines.Add($"{PostalCode} {City}");
        addressLines.Add(Region);
        addressLines.Add(CountryCode);
        return string.Join(Environment.NewLine, addressLines);
    }
}

