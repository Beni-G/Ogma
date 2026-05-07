using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Partners.Entities;
public class PartnerIdentifier : Entity<long>
{
    public string Type { get; private set; }
    public string Value { get; private set; }
    public Period? ValidityPeriod { get; private set; }
    public bool IsPrimary { get; private set; } = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="PartnerIdentifier"/> class with the specified partner ID, type,
    /// value, validity period, and primary status.
    /// </summary>
    /// <param name="type">The type of the identifier. Cannot be null or empty.</param>
    /// <param name="value">The value of the identifier. Cannot be null or empty.</param>
    /// <param name="validityPeriod">The optional period during which the identifier is valid. If not specified, the identifier is considered valid
    /// indefinitely.</param>
    /// <param name="isPrimary">A boolean value indicating whether this identifier is the primary one for the partner. <see langword="true"/> if
    /// it is primary; otherwise, <see langword="false"/>.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="type"/> or <paramref name="value"/> is null or empty.</exception>
    private PartnerIdentifier(string type, string value, Period? validityPeriod = null, bool isPrimary = false)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException("Identifier type cannot be null or empty.", nameof(type));
        }
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Identifier value cannot be null or empty.", nameof(value));
        }
        Type = type;
        Value = value;
        ValidityPeriod = validityPeriod;
        IsPrimary = isPrimary;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PartnerIdentifier"/> class with the specified identifier details.
    /// </summary>
    /// <param name="id">The unique identifier for the partner identifier. Must be a positive number.</param>
    /// <param name="type">The type of the identifier. Cannot be null or empty.</param>
    /// <param name="value">The value of the identifier. Cannot be null or empty.</param>
    /// <param name="metadata">Metadata for the partner identifier.</param>
    /// <param name="validityPeriod">The optional validity period for the identifier. If not specified, the identifier is considered to have no
    /// expiration.</param>
    /// <param name="isPrimary">Indicates whether this identifier is the primary identifier for the partner. Defaults to <see
    /// langword="false"/>.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="id"/>, <paramref name="type"/>, or <paramref
    /// name="value"/> do not meet the specified conditions.</exception>
    private PartnerIdentifier(long id, string type, string value, EntityMetadata metadata, Period? validityPeriod = null, bool isPrimary = false) : base(id, metadata)
    {
        
        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException("Identifier type cannot be null or empty.", nameof(type));
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Identifier value cannot be null or empty.", nameof(value));
        }
        Type = type;
        Value = value;
        ValidityPeriod = validityPeriod;
        IsPrimary = isPrimary;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PartnerIdentifier"/> class with the specified parameters.
    /// </summary>
    /// <param name="type">The type of the partner identifier. Cannot be null or empty.</param>
    /// <param name="value">The value of the partner identifier. Cannot be null or empty.</param>
    /// <param name="validityPeriod">The optional validity period for the partner identifier. If not specified, the identifier is considered valid
    /// indefinitely.</param>
    /// <param name="isPrimary">A boolean value indicating whether this identifier is the primary one for the partner. <see langword="true"/> if
    /// it is primary; otherwise, <see langword="false"/>.</param>
    /// <returns>A new <see cref="PartnerIdentifier"/> instance initialized with the specified parameters.</returns>
    public static PartnerIdentifier Create(string type, string value, Period? validityPeriod = null, bool isPrimary = false) 
        => new(type, value, validityPeriod, isPrimary);

    /// <summary>
    /// Reconstitutes a <see cref="PartnerIdentifier"/> instance with the specified parameters.
    /// </summary>
    /// <param name="id">The unique identifier for the partner.</param>
    /// <param name="type">The type of the partner identifier.</param>
    /// <param name="value">The value of the partner identifier.</param>
    /// <param name="metadata">Metadata for the partner identifier.</param>
    /// <param name="validityPeriod">The optional validity period for the partner identifier. Defaults to <see langword="null"/> if not specified.</param>
    /// <param name="isPrimary">A value indicating whether this identifier is the primary one. Defaults to <see langword="false"/>.</param>
    /// <returns>A <see cref="PartnerIdentifier"/> instance initialized with the provided parameters.</returns>
    public static PartnerIdentifier Reconstitute(long id, string type, string value, EntityMetadata metadata, Period? validityPeriod = null, bool isPrimary = false) 
        => new(id, type, value, metadata, validityPeriod, isPrimary);

    /// <summary>
    /// Updates the identifier with the specified type, value, and optional validity period.
    /// </summary>
    /// <param name="type">The type of the identifier. Cannot be null or empty.</param>
    /// <param name="value">The value of the identifier. Cannot be null or empty.</param>
    /// <param name="validityPeriod">The optional period during which the identifier is valid. If not specified, the identifier is considered to have
    /// no expiration.</param>
    /// <param name="isPrimary">Indicates whether the identifier is the primary one. <see langword="true"/> if it is primary; otherwise, <see
    /// langword="false"/>.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="type"/> or <paramref name="value"/> is null or empty.</exception>
    public void Update(string type, string value, Period? validityPeriod = null, bool isPrimary = false)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException("Identifier type cannot be null or empty.", nameof(type));
        }
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Identifier value cannot be null or empty.", nameof(value));
        }
        Type = type;
        Value = value;
        ValidityPeriod = validityPeriod;
        IsPrimary = isPrimary;
        Touch();
    }

    /// <summary>
    /// Marks the current instance as the primary instance.
    /// </summary>
    /// <remarks>This method sets the <see cref="IsPrimary"/> property to <see langword="true"/>, indicating
    /// that the current instance is the primary one.</remarks>
    public void MarkAsPrimary() => IsPrimary = true;

    /// <summary>
    /// Unmarks the current instance as the primary entity.
    /// </summary>
    /// <remarks>Sets the <see cref="IsPrimary"/> property to <see langword="false"/>, indicating that this
    /// instance is no longer the primary entity.</remarks>
    public void UnmarkAsPrimary() => IsPrimary = false;

    /// <summary>
    /// Returns a boolean indicating whether the identifier is valid on the specified date.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public bool IsValidOn(DateTime date) => ValidityPeriod?.Contains(date) ?? true;

    /// <summary>
    /// Returns a boolean indicating whether the identifier is currently valid.
    /// </summary>
    /// <returns></returns>
    public bool IsCurrentlyValid() => ValidityPeriod?.Contains(DateTime.Today) ?? true;
}
