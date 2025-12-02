using Ogma.Domain.SharedKernel.BaseTypes;
using System.Text.RegularExpressions;

namespace Ogma.Domain.SharedKernel.ValueObjects;

public sealed class Email : ValueObject
{
    private const int MaxLength = 254;

    private static readonly Regex EmailRegex = new(
        @"^[\w!#$%&'*+/=?`{|}~^-]+(?:\.[\w!#$%&'*+/=?`{|}~^-]+)*@(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromSeconds(1));

    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(value));
        }

        var trimmed = value.Trim();

        if (trimmed.Length > MaxLength)
        {
            throw new ArgumentException($"Email cannot exceed {MaxLength} characters.", nameof(value));
        }

        // This one blocks bad..dots@example.com 100%
        if (!EmailRegex.IsMatch(trimmed))
        {
            throw new ArgumentException("Invalid email format.", nameof(value));
        }

        Value = trimmed.ToLowerInvariant();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}