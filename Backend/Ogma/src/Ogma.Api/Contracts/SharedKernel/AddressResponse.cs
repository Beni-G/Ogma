namespace Ogma.Api.Contracts.SharedKernel;

public record AddressResponse(
    string Street,
    string Number,
    string City,
    string Region,
    string PostalCode,
    string CountryCode,
    string? Building,
    string? StairCase,
    string? Floor,
    string? Apartment);