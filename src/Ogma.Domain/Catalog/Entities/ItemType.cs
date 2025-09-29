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
            throw new ArgumentNullException("name");
        }

        Name = name;
        Description = description;
    }

    /// <summary>
    /// Creates a new instance of the ItemType class with the specified ID, name, and description.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private ItemType(long id, string name, string description) : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException("name");
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
    /// Reconstitutes a ItemType instance from existing data.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public static ItemType Reconstitute(long id, string name, string description) => new(id, name, description);
}
