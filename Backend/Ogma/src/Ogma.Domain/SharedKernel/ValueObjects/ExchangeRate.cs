using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Domain.SharedKernel.ValueObjects;

public class ExchangeRate : ValueObject
{
    public string BaseCurrency { get; }
    public string TargetCurrency { get; }
    public decimal Rate { get; }

    public ExchangeRate(string baseCurrency, string targetCurrency, decimal rate)
    {
        if (rate <= 0)
        {
            throw new ArgumentException("Exchange rate must be greater than zero.", nameof(rate));
        }

        if (string.IsNullOrWhiteSpace(baseCurrency))
        {
            throw new ArgumentException("Base currency cannot be null or empty.", nameof(baseCurrency));
        }

        if (string.IsNullOrWhiteSpace(targetCurrency))
        {
            throw new ArgumentException("Target currency cannot be null or empty.", nameof(targetCurrency));
        }

        BaseCurrency = baseCurrency.ToUpperInvariant();
        TargetCurrency = targetCurrency.ToUpperInvariant();
        Rate = rate;
    }

    public Money Convert(Money amount)
    {
        if (amount.Currency != BaseCurrency)
        {
            throw new ArgumentException($"Amount currency must be {BaseCurrency} to convert.", nameof(amount));
        }
        decimal convertedAmount = amount.Amount * Rate;
        return new Money(convertedAmount, TargetCurrency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return BaseCurrency;
        yield return TargetCurrency;
        yield return Rate;
    }

    public override string ToString() => $"1 {BaseCurrency} = {Rate} {TargetCurrency}";
}
