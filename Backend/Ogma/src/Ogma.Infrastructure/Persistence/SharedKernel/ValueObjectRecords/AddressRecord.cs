using System.ComponentModel.DataAnnotations.Schema;

namespace Ogma.Infrastructure.Persistence.SharedKernel.ValueObjectRecords;

[ComplexType]
public record AddressRecord(
    string Street,
    string Number,
    string City,
    string Region,
    string PostalCode,
    string CountryCode,
    string? Building = null,
    string? Staircase = null,
    string? Floor = null,
    string? Apartment = null
);
