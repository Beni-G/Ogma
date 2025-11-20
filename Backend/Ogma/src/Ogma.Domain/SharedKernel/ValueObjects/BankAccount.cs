using Ogma.Domain.SharedKernel.BaseTypes;
using System.Text;

namespace Ogma.Domain.SharedKernel.ValueObjects;
public class BankAccount : ValueObject
{
    public string Bank { get; }
    public string Iban { get; }
    public string Currency { get; }
    public string? Bic { get; }

    public BankAccount(string bank, string iban, string currency, string? bic = null)
    {
        if (string.IsNullOrWhiteSpace(bank))
        {
            throw new ArgumentNullException(nameof(bank));
        }
        if (string.IsNullOrWhiteSpace(iban))
        {
            throw new ArgumentNullException(nameof(iban));
        }
        if(!IbanChecksumIsValid(iban))
        {
            throw new ArgumentException("IBAN checksum is invalid.", nameof(iban));
        }
        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentNullException(nameof(currency));
        }
        Bank = bank;
        Iban = iban;
        Currency = currency.ToUpperInvariant();
        Bic = bic;
    }

    private static bool IbanChecksumIsValid(string iban)
    {
        // Step 1: Move first 4 characters to the end
        string rearranged = iban.Substring(4) + iban.Substring(0, 4);

        // Step 2: Convert letters to numbers (A=10 -> Z=35)
        var sb = new StringBuilder();

        foreach (char c in rearranged)
        {
            if (char.IsLetter(c))
            {
                int value = char.ToUpper(c) - 'A' + 10;
                sb.Append(value);
            }
            else if (char.IsDigit(c))
            {
                sb.Append(c);
            }
            else
            {
                return false; // invalid characters
            }
        }

        // Step 3+4: Modulo 97, but the number can be too big for ulong,
        // so process in chunks.
        string chunk = sb.ToString();
        int mod = 0;

        foreach (char ch in chunk)
        {
            mod = (mod * 10 + (ch - '0')) % 97;
        }

        // Step 5: Valid IBAN ⇒ remainder == 1
        return mod == 1;
    }


    public string GetFormattedIban()
    {
        return string.Join(" ", Enumerable
            .Range(0, (Iban.Length + 3) / 4)
            .Select(i => Iban.Substring(i * 4, Math.Min(4, Iban.Length - i * 4))));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Bank;
        yield return Iban;  
        yield return Currency;
        yield return Bic!;
    }

    public override string ToString() => $"{Bank} - {GetFormattedIban()} ({Currency})";
}
