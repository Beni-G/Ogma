using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Domain.Partners.Entities;
public class PartnerRole : AggregateRoot<long>
{
    public string Code { get; private set; }
    public string Name { get; private set; }
    public string? Color { get; private set; }


    /// <summary>
    /// Initializes a new instance of the <see cref="PartnerRole"/> class with the specified code, name, and optional
    /// color.
    /// </summary>
    /// <param name="code">The unique code representing the partner role. Cannot be null or empty.</param>
    /// <param name="name">The name of the partner role. Cannot be null or empty.</param>
    /// <param name="color">The optional color associated with the partner role. Can be null.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="code"/> or <paramref name="name"/> is null or empty.</exception>
    private PartnerRole(string code, string name, string color = null)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Code cannot be null or empty.", nameof(code));
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }
        Code = code;
        Name = name;
        Color = color;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PartnerRole"/> class with the specified identifier, code, name, and
    /// optional color.
    /// </summary>
    /// <param name="id">The unique identifier for the partner role.</param>
    /// <param name="code">The code representing the partner role. Cannot be null or empty.</param>
    /// <param name="name">The name of the partner role. Cannot be null or empty.</param>
    /// <param name="color">The optional color associated with the partner role. Can be null.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="code"/> or <paramref name="name"/> is null or empty.</exception>
    private PartnerRole(long id, string code, string name, string color = null) : base(id)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Code cannot be null or empty.", nameof(code));
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }
        Code = code;
        Name = name;
        Color = color;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PartnerRole"/> class with the specified code, name, and optional color.
    /// </summary>
    /// <param name="code">The unique code representing the partner role. Cannot be null or empty.</param>
    /// <param name="name">The name of the partner role. Cannot be null or empty.</param>
    /// <param name="color">The optional color associated with the partner role. If not specified, defaults to null.</param>
    /// <returns>A new <see cref="PartnerRole"/> instance initialized with the provided code, name, and color.</returns>
    public static PartnerRole Create(string code, string name, string color = null) => new(code, name, color);

    /// <summary>
    /// Recreates a <see cref="PartnerRole"/> instance with the specified attributes.
    /// </summary>
    /// <param name="id">The unique identifier for the partner role.</param>
    /// <param name="code">The code representing the partner role.</param>
    /// <param name="name">The name of the partner role.</param>
    /// <param name="color">The optional color associated with the partner role. Can be <see langword="null"/>.</param>
    /// <returns>A <see cref="PartnerRole"/> object initialized with the provided attributes.</returns>
    public static PartnerRole Reconstitute(long id, string code, string name, string color = null) => new(id, code, name, color);

}
