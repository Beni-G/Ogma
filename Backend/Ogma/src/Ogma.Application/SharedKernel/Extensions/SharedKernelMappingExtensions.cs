using Ogma.Application.SharedKernel.Dtos;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Application.SharedKernel.Extensions;

public static class SharedKernelMappingExtensions
{
    #region ToDto Methods
    public static AddressDto ToDto(this Address address)
    {
        return new AddressDto(
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
    }

    public static BankAccountDto ToDto(this BankAccount bankAccount) =>
        new BankAccountDto(bankAccount.Bank, bankAccount.Iban, bankAccount.Currency, bankAccount.Bic);

    public static MoneyDto ToDto(this Money money) => new (money.Amount, money.Currency);

    public static PeriodDto ToDto(this Period period) => new (period.Start, period.End);

    public static PersonNameDto ToDto(this PersonName personName) => new (personName.FirstName, personName.LastName);

    public static ExchangeRateDto ToDto(this ExchangeRate exchangeRate) => new (exchangeRate.BaseCurrency, exchangeRate.TargetCurrency, exchangeRate.Rate);

    #endregion

    #region ToDomain Methods

    public static Address ToDomain(this AddressDto addressDto)
    {
        return new Address(
            addressDto.Street,
            addressDto.Number,
            addressDto.City,
            addressDto.Region,
            addressDto.PostalCode,
            addressDto.CountryCode,
            addressDto.Building,
            addressDto.Staircase,
            addressDto.Floor,
            addressDto.Apartment);
    }

    public static BankAccount ToDomain(this BankAccountDto bankAccountDto) =>
        new (bankAccountDto.Bank, bankAccountDto.Iban, bankAccountDto.Currency, bankAccountDto.Bic);

    public static Money ToDomain(this MoneyDto moneyDto) => new (moneyDto.Amount, moneyDto.Currency);

    public static Period ToDomain(this PeriodDto periodDto) => new (periodDto.Start, periodDto.End);

    public static PersonName ToDomain(this PersonNameDto personNameDto) => new (personNameDto.FirstName, personNameDto.LastName);

    public static ExchangeRate ToDomain(this ExchangeRateDto exchangeRateDto) => new (exchangeRateDto.BaseCurrency, exchangeRateDto.TargetCurrency, exchangeRateDto.Rate);

    #endregion
}
