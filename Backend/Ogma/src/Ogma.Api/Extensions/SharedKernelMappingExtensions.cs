using Ogma.Api.Contracts.SharedKernel;
using Ogma.Application.SharedKernel.Dtos;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Api.Extensions;

public static class SharedKernelMappingExtensions
{
    public static AddressResponse ToResponse(this Address dto) =>
        new (
            dto.Street,
            dto.Number,
            dto.City,
            dto.Region,
            dto.PostalCode,
            dto.CountryCode,
            dto.Building,
            dto.Staircase,
            dto.Floor,
            dto.Apartment);

    public static AddressResponse ToResponse(this AddressDto address) =>
        new (
            address.Street,
            address.Number,
            address.City,
            address.Region,
            address.PostalCode,
            address.CountryCode,
            address.Building,
            address.Staircase,
            address.Floor,
            address.Apartment);

    public static PersonNameResponse ToResponse(this PersonName personName) => new (personName.FirstName, personName.LastName);

    public static PersonNameResponse ToResponse(this PersonNameDto personName) => new (personName.FirstName, personName.LastName);

    public static PeriodResponse ToResponse(this Period period) => new (period.Start, period.End);

    public static PeriodResponse ToResponse(this PeriodDto period) => new (period.Start, period.End);

    public static BankAccountResponse ToResponse(this BankAccount bankAccount) => new (bankAccount.Bank, bankAccount.Iban, bankAccount.Currency, bankAccount.Bic);

    public static BankAccountResponse ToResponse(this BankAccountDto bankAccount) => new (bankAccount.Bank, bankAccount.Iban, bankAccount.Currency, bankAccount.Bic);

    public static MoneyResponse ToResponse(this Money money) => new (money.Amount, money.Currency);

    public static MoneyResponse ToResponse(this MoneyDto money) => new (money.Amount, money.Currency);

    public static ExchangeRateResponse ToResponse(this ExchangeRate exchangeRate) => new (exchangeRate.BaseCurrency, exchangeRate.TargetCurrency, exchangeRate.Rate);

    public static ExchangeRateResponse ToResponse(this ExchangeRateDto exchangeRate) => new(exchangeRate.BaseCurrency, exchangeRate.TargetCurrency, exchangeRate.Rate);
}
