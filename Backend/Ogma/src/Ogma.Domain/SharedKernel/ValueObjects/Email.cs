using Ogma.Domain.SharedKernel.BaseTypes;
using System.Text.RegularExpressions;

namespace Ogma.Domain.SharedKernel.ValueObjects;
public class Email : ValueObject
{
    private const int MaxLength = 254;
    private const string Pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(value));
        }
        var cleanedValue = value.Trim();
        if (cleanedValue.Length > MaxLength)
        {
            throw new ArgumentException("Email cannot exceed 254 characters.", nameof(value));
        }
        if (!Regex.IsMatch(cleanedValue, Pattern))
        {
            throw new ArgumentException("Invalid email format.", nameof(value));
        }
        Value = value.Trim().ToLowerInvariant();
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
