using Ogma.Domain.Catalog.Parameters;
using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Catalog.Entities;
public class Item : AggregateRoot<long>
{
    public string Name { get; private set; }
    public string Code { get; private set; }
    public string Description { get; private set; }
    public Category Category { get; private set; }
    public Money ListPrice { get; private set; }
    public ItemType ItemType { get; private set; }
    public string UnitOfMeasurement { get; private set; }
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creates a new instance of the Item class with the specified parameters.
    /// </summary>
    /// <param name="itemParameters"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    private Item(ItemParameters itemParameters)
    {
        if(itemParameters == null)
        {
            throw new ArgumentNullException(nameof(itemParameters));
        }

        if (string.IsNullOrWhiteSpace(itemParameters.Name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(itemParameters.Name));
        }

        if (string.IsNullOrWhiteSpace(itemParameters.Code))
        {
            throw new ArgumentException("Code cannot be null or empty.", nameof(itemParameters.Code));
        }

        if(string.IsNullOrWhiteSpace(itemParameters.UnitOfMeasurement))
        {
            throw new ArgumentException("Unit of Measurement cannot be null or empty.", nameof(itemParameters.UnitOfMeasurement));
        }

        Name = itemParameters.Name;
        Code = itemParameters.Code;
        Description = itemParameters.Description;
        Category = itemParameters.Category ?? throw new ArgumentNullException(nameof(itemParameters.Category));
        ListPrice = itemParameters.ListPrice ?? throw new ArgumentNullException(nameof(itemParameters.ListPrice));
        ItemType = itemParameters.ItemType ?? throw new ArgumentNullException(nameof(itemParameters.ItemType));
        UnitOfMeasurement = itemParameters.UnitOfMeasurement;
        IsActive = itemParameters.IsActive;
    }

    /// <summary>
    /// Creates a new instance of the Item class with the specified ID and parameters.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="itemParameters"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    private Item(long id, ItemParameters itemParameters) : base(id)
    {
        if (itemParameters == null)
        {
            throw new ArgumentNullException(nameof(itemParameters));
        }

        if (string.IsNullOrWhiteSpace(itemParameters.Name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(itemParameters.Name));
        }

        if (string.IsNullOrWhiteSpace(itemParameters.Code))
        {
            throw new ArgumentException("Code cannot be null or empty.", nameof(itemParameters.Code));
        }

        if (string.IsNullOrWhiteSpace(itemParameters.UnitOfMeasurement))
        {
            throw new ArgumentException("Unit of Measurement cannot be null or empty.", nameof(itemParameters.UnitOfMeasurement));
        }

        Name = itemParameters.Name;
        Code = itemParameters.Code;
        Description = itemParameters.Description;
        Category = itemParameters.Category ?? throw new ArgumentNullException(nameof(itemParameters.Category));
        ListPrice = itemParameters.ListPrice ?? throw new ArgumentNullException(nameof(itemParameters.ListPrice));
        ItemType = itemParameters.ItemType ?? throw new ArgumentNullException(nameof(itemParameters.ItemType));
        UnitOfMeasurement = itemParameters.UnitOfMeasurement;
        IsActive = itemParameters.IsActive;
    }

    /// <summary>
    /// Instantiates a new Item using the provided parameters.
    /// </summary>
    /// <param name="itemParameters"></param>
    /// <returns></returns>
    public static Item Create(ItemParameters itemParameters) => new(itemParameters);

    /// <summary>
    /// Reconstitutes an Item from the given ID and parameters.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="itemParameters"></param>
    /// <returns></returns>
    public static Item Reconstitute(long id, ItemParameters itemParameters) => new(id, itemParameters);

    /// <summary>
    /// Updates the item's properties based on the provided parameters.
    /// </summary>
    /// <param name="itemParameters"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void UpdateItem(ItemParameters itemParameters)
    {
        if (itemParameters == null)
        {
            throw new ArgumentNullException(nameof(itemParameters));
        }
        UpdateName(itemParameters.Name);
        UpdateCode(itemParameters.Code);
        UpdateDescription(itemParameters.Description);
        UpdateCategory(itemParameters.Category);
        UpdateListPrice(itemParameters.ListPrice);
        UpdateItemType(itemParameters.ItemType);
        UpdateUnitOfMeasurement(itemParameters.UnitOfMeasurement);
        if (itemParameters.IsActive)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

    /// <summary>
    /// Updates the name of the item.
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="ArgumentException"></exception>
    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }

        Name = name;
    }

    /// <summary>
    /// Updates the code of the item.
    /// </summary>
    /// <param name="code"></param>
    /// <exception cref="ArgumentException"></exception>
    public void UpdateCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Code cannot be null or empty.", nameof(code));
        }
        Code = code;
    }

    /// <summary>
    /// Updates the description of the item.
    /// </summary>
    /// <param name="description"></param>
    public void UpdateDescription(string description) => Description = description;

    public void UpdateCategory(Category category)
    {
        Category = category ?? throw new ArgumentNullException(nameof(category));
    }

    /// <summary>
    /// Updates the list price of the item.
    /// </summary>
    /// <param name="newPrice"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void UpdateListPrice(Money newPrice)
    {
        ListPrice = newPrice ?? throw new ArgumentNullException(nameof(newPrice));
    }

    /// <summary>
    /// Updates the item type of the item.
    /// </summary>
    /// <param name="itemType"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void UpdateItemType(ItemType itemType)
    {
        ItemType = itemType ?? throw new ArgumentNullException(nameof(itemType));
    }

    /// <summary>
    /// Updates the unit of measurement for the item.
    /// </summary>
    /// <param name="unitOfMeasurement"></param>
    /// <exception cref="ArgumentException"></exception>
    public void UpdateUnitOfMeasurement(string unitOfMeasurement)
    {
        if (string.IsNullOrWhiteSpace(unitOfMeasurement))
        {
            throw new ArgumentException("Unit of Measurement cannot be null or empty.", nameof(unitOfMeasurement));
        }
        UnitOfMeasurement = unitOfMeasurement;
    }

    /// <summary>
    /// Deactivates the item, setting its active state to false.
    /// </summary>
    public void Deactivate() => IsActive = false;

    /// <summary>
    /// Activates the item, setting its active state to true.
    /// </summary>
    public void Activate() => IsActive = true;
}
