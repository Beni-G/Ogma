using FluentAssertions;
using Ogma.Domain.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ogma.Domain.UnitTests.SharedKernel.ValueObjects;

public class AddressTests
{
    [Fact]
    public void Constructor_ValidValuesProvided_SetsPropertiesCorrectly()
    {
        // Arrange
        var street = "Main St.";
        var number = "1A";
        var city = "Gotham";
        var region = "Gotham State";
        var postalCode = "12345";
        var countryCode = "US";
        var building = "Abc";
        var staircase = "D";
        var floor = "F";
        var apartment = "12";

        // Act
        var address = new Address(
            street,
            number,
            city,
            region,
            postalCode,
            countryCode,
            building,
            staircase,
            floor,
            apartment);

        // Assert
        address.Street.Should().Be(street);
        address.Number.Should().Be(number);
        address.City.Should().Be(city);
        address.Region.Should().Be(region);
        address.PostalCode.Should().Be(postalCode);
        address.CountryCode.Should().Be(countryCode);
        address.Building.Should().Be(building);
        address.Staircase.Should().Be(staircase);
        address.Floor.Should().Be(floor);
        address.Apartment.Should().Be(apartment);
    }

    [Fact]
    public void Constructor_NullOptionalParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var street = "Main St.";
        var number = "1A";
        var city = "Gotham";
        var region = "Gotham State";
        var postalCode = "12345";
        var countryCode = "US";

        // Act
        var address = new Address(
            street,
            number,
            city,
            region,
            postalCode,
            countryCode);

        // Assert
        address.Street.Should().Be(street);
        address.Number.Should().Be(number);
        address.City.Should().Be(city);
        address.Region.Should().Be(region);
        address.PostalCode.Should().Be(postalCode);
        address.CountryCode.Should().Be(countryCode);
        address.Building.Should().BeNull();
        address.Staircase.Should().BeNull();
        address.Floor.Should().BeNull();
        address.Apartment.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidStreet_ThrowsArgumentException_WithCorrectParamName(string? street)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Address(street, "1A", "Gotham", "GS", "12345", "US"));

        Assert.Equal("street", ex.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidNumber_ThrowsArgumentException_WithCorrectParamName(string? number)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Address("Main St", number, "Gotham", "GS", "12345", "US"));

        Assert.Equal("number", ex.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidCity_ThrowsArgumentException_WithCorrectParamName(string? city)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Address("Main St", "1A", city, "GS", "12345", "US"));

        Assert.Equal("city", ex.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidRegion_ThrowsArgumentException_WithCorrectParamName(string? region)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Address("Main St", "1A", "Gotham", region, "12345", "US"));

        Assert.Equal("region", ex.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_InvalidPostalCode_ThrowsArgumentException_WithCorrectParamName(string? postalCode)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Address("Main St", "1A", "Gotham", "GS", postalCode, "US"));

        Assert.Equal("postalCode", ex.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("U")]
    [InlineData("USA")]
    [InlineData("12")]
    [InlineData("U1")]
    public void Constructor_InvalidCountryCode_ThrowsArgumentException_WithCorrectParamName(string? countryCode)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Address("Main St", "1A", "Gotham", "GS", "12345", countryCode));

        Assert.Equal("countryCode", ex.ParamName);
    }

    [Fact]
    public void Constructor_ValidData_TrimsAndNormalizesCorrectly()
    {
        var address = new Address(
            street: "  Baker Street  ",
            number: "  221B  ",
            city: " London ",
            region: "Greater London  ",
            postalCode: " NW1 6XE ",
            countryCode: "gb",
            building: "  Sherlock House  ",
            apartment: "  2  ");

        Assert.Equal("Baker Street", address.Street);
        Assert.Equal("221B", address.Number);
        Assert.Equal("London", address.City);
        Assert.Equal("Greater London", address.Region);
        Assert.Equal("NW1 6XE", address.PostalCode);
        Assert.Equal("GB", address.CountryCode);
        Assert.Equal("Sherlock House", address.Building);
        Assert.Equal("2", address.Apartment);
    }
}
