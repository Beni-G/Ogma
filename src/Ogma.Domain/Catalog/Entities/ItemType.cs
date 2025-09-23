using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Domain.Catalog.Entities;
public class ItemType : AggregateRoot<long>
{
    public string Name { get; private set; }
    public string Description { get; private set; }

    public ItemType(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException("name");
        }

        Name = name;
        Description = description;
    }
}
