using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Domain.Orders.Entities;

public class OrderStatus : AggregateRoot<long>
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    private OrderStatus(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }

        Name = name;
        Description = description;
    }

    private OrderStatus(long id, string name, string description, EntityMetadata metadata) : base(id, metadata)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Creates a new instance of the OrderStatus class with the specified name and description.
    /// </summary>
    /// <param name="name">The unique name that identifies the order status. Cannot be null or empty.</param>
    /// <param name="description">A description providing additional details about the order status. Can be null or empty if no description is
    /// required.</param>
    /// <returns>A new OrderStatus instance initialized with the specified name and description.</returns>
    public static OrderStatus Create(string name, string description) => new(name, description);

    /// <summary>
    /// Recreates an instance of the OrderStatus class from the specified identifier, name, and description.
    /// </summary>
    /// <param name="id">The unique identifier for the order status.</param>
    /// <param name="name">The name of the order status. Cannot be null or empty.</param>
    /// <param name="description">A description of the order status. Can be null or empty if no description is available.</param>
    /// <param name="metadata">The metadata for the reconstituted order statis.</param>
    /// <returns>An OrderStatus instance initialized with the provided identifier, name, and description.</returns>
    public static OrderStatus Reconstitute(long id, string name, string description, EntityMetadata metadata) => new(id, name, description, metadata);

    /// <summary>
    /// Updates the name and description of the current instance.
    /// </summary>
    /// <param name="name">The new name to assign. Cannot be null, empty, or consist only of white-space characters.</param>
    /// <param name="description">The new description to assign. Can be null or empty.</param>
    /// <exception cref="ArgumentException">Thrown if the value of <paramref name="name"/> is null, empty, or consists only of white-space characters.</exception>
    public void Update(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }
        Name = name;
        Description = description;
        Touch();
    }
}
