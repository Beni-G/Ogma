using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Domain.Catalog.Entities;
public class ItemType : AggregateRoot<long>
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    /// <summary>
    /// Creates a new instance of the ItemType class with the specified name and description.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private ItemType(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("name");
        }

        Name = name;
        Description = description;
    }

    /// <summary>
    /// Creates a new instance of the ItemType class with the specified ID, name description and metadata.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// /// <param name="metadata"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private ItemType(long id, string name, string description, EntityMetadata metadata) : base(id, metadata)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("ItemType name is invalid.", nameof(name));
        }

        Name = name;
        Description = description;
    }

    /// <summary>
    /// Creates a new ItemType instance.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public static ItemType Create(string name, string description) => new(name, description);

    /// <summary>
    /// Reconstitutes the ItemType class with the specified ID, name description and metadata.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    public static ItemType Reconstitute(long id, string name, string description, EntityMetadata metadata) => 
        new(id, name, description, metadata);

    /// <summary>
    /// Updates the Name and Description of the ItemType.
    /// </summary>
    /// <param name="name"></param>
    public void Update(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("ItemType name is invalid.", nameof(name));
        }

        Name = name;
        Description = description;

        Touch();
    }
}
