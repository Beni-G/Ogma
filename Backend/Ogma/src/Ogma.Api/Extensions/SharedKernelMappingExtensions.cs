using Ogma.Api.Contracts.SharedKernel;
using Ogma.Application.SharedKernel.Dtos;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Api.Extensions;

public static class SharedKernelMappingExtensions
{
    public static AddressResponse ToResponse(this Address dto) =>
        new AddressResponse(
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
        new AddressResponse(
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

    public static PersonNameResponse ToResponse(this PersonName personName) =>
        new PersonNameResponse(personName.FirstName, personName.LastName);

    public static PersonNameResponse ToResponse(this PersonNameDto personName) =>
        new PersonNameResponse(personName.FirstName, personName.LastName);

    public static PeriodResponse ToResponse(this Period period) =>
        new PeriodResponse(period.Start, period.End);

    public static PeriodResponse ToResponse(this PeriodDto period) =>
        new PeriodResponse(period.Start, period.End);

    public static BankAccountResponse ToResponse(this BankAccount bankAccount) =>
        new BankAccountResponse(bankAccount.Bank, bankAccount.Iban, bankAccount.Currency, bankAccount.Bic);

    public static BankAccountResponse ToResponse(this BankAccountDto bankAccount) =>
        new BankAccountResponse(bankAccount.Bank, bankAccount.Iban, bankAccount.Currency, bankAccount.Bic);

    public static MoneyResponse ToResponse(this Money money) =>
        new MoneyResponse(money.Amount, money.Currency);

    public static MoneyResponse ToResponse(this MoneyDto money) =>
        new MoneyResponse(money.Amount, money.Currency);
}
