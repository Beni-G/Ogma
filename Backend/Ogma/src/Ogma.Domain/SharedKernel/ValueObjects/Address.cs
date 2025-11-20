using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Domain.SharedKernel.ValueObjects;
public class Address : ValueObject
{
    public string Street { get; }
    public string Number { get; }
    public string City { get; }
    public string Region { get; }
    public string PostalCode { get; }
    public string CountryCode { get; } // ISO 3166-1 alpha-2, e.g., "DE"

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
        Street = street ?? throw new ArgumentNullException(nameof(street));
        Number = number ?? throw new ArgumentNullException(nameof(number));
        City = city ?? throw new ArgumentNullException(nameof(city));
        Region = region ?? throw new ArgumentNullException(nameof(region));
        PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));

        if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2)
        {
            throw new ArgumentException("Country code must be a 2-letter ISO code.", nameof(countryCode));
        }

        CountryCode = countryCode.ToUpperInvariant();

        Building = building;
        Staircase = staircase;
        Floor = floor;
        Apartment = apartment;
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

