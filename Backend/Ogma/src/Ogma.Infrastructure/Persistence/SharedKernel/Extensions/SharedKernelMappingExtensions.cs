using Ogma.Application.SharedKernel.Dtos;
using Ogma.Infrastructure.Persistence.SharedKernel.ValueObjectRecords;

namespace Ogma.Infrastructure.Persistence.SharedKernel.Extensions;

public static class SharedKernelMappingExtensions
{
    #region ToDto

    public static BankAccountDto ToDto(this BankAccountRecord bankAccountRecord)
    {
        ArgumentNullException.ThrowIfNull(bankAccountRecord, nameof(bankAccountRecord));

        return new BankAccountDto(
            Bank: bankAccountRecord.Bank,
            Iban: bankAccountRecord.Iban,
            Currency: bankAccountRecord.Currency,
            Bic: bankAccountRecord.Bic
        );
    }

    public static AddressDto ToDto(this AddressRecord addressRecord)
    {
        ArgumentNullException.ThrowIfNull(addressRecord, nameof(addressRecord));

        return new AddressDto(
            Street: addressRecord.Street,
            Number: addressRecord.Number,
            City: addressRecord.City,
            Region: addressRecord.Region,
            PostalCode: addressRecord.PostalCode,
            CountryCode: addressRecord.CountryCode,
            Building: addressRecord.Building,
            Staircase: addressRecord.Staircase,
            Floor: addressRecord.Floor,
            Apartment: addressRecord.Apartment
        );
    }

    #endregion
}
