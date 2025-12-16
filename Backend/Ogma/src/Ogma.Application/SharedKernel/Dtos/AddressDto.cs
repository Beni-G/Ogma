namespace Ogma.Application.SharedKernel.Dtos;

public record AddressDto(
    string Street,
    string Number,
    string City,
    string Region,
    string PostalCode,
    string CountryCode,
    string? Building,
    string? Staircase,
    string? Floor,
    string? Apartment);
