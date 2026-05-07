using Ogma.Domain.Catalog.Parameters;
using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.Catalog.Entities;
public class Item : AggregateRoot<long>
{
    public string Name { get; private set; }
    public string Code { get; private set; }
    public string Description { get; private set; }
    public long CategoryId { get; private set; }
    public Money ListPrice { get; private set; }
    public long ItemTypeId { get; private set; }
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

        if (itemParameters.CategoryId <= 0)
        {
            throw new ArgumentException("Category ID must be a positive number.", nameof(itemParameters.CategoryId));
        }

        if (itemParameters.ItemTypeId <= 0)
        {
            throw new ArgumentException("Item Type ID must be a positive number.", nameof(itemParameters.ItemTypeId));
        }

        if (string.IsNullOrWhiteSpace(itemParameters.UnitOfMeasurement))
        {
            throw new ArgumentException("Unit of Measurement cannot be null or empty.", nameof(itemParameters.UnitOfMeasurement));
        }

        Name = itemParameters.Name;
        Code = itemParameters.Code;
        Description = itemParameters.Description;
        CategoryId = itemParameters.CategoryId;
        ListPrice = itemParameters.ListPrice ?? throw new ArgumentNullException(nameof(itemParameters.ListPrice));
        ItemTypeId = itemParameters.ItemTypeId;
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
    private Item(long id, ItemParameters itemParameters, EntityMetadata metadata) : base(id, metadata)
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

        if (itemParameters.CategoryId <= 0)
        {
            throw new ArgumentException("Category ID must be a positive number.", nameof(itemParameters.CategoryId));
        }

        if (itemParameters.ItemTypeId <= 0)
        {
            throw new ArgumentException("Item Type ID must be a positive number.", nameof(itemParameters.ItemTypeId));
        }

        if (string.IsNullOrWhiteSpace(itemParameters.UnitOfMeasurement))
        {
            throw new ArgumentException("Unit of Measurement cannot be null or empty.", nameof(itemParameters.UnitOfMeasurement));
        }

        Name = itemParameters.Name;
        Code = itemParameters.Code;
        Description = itemParameters.Description;
        CategoryId = itemParameters.CategoryId;
        ListPrice = itemParameters.ListPrice ?? throw new ArgumentNullException(nameof(itemParameters.ListPrice));
        ItemTypeId = itemParameters.ItemTypeId;
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
    /// Reconstitutes an Item from the given ID parameters and metadata.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="itemParameters"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    public static Item Reconstitute(long id, ItemParameters itemParameters, EntityMetadata metadata) => new(id, itemParameters, metadata);

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
        UpdateCategoryId(itemParameters.CategoryId);
        UpdateListPrice(itemParameters.ListPrice);
        UpdateItemTypeId(itemParameters.ItemTypeId);
        UpdateUnitOfMeasurement(itemParameters.UnitOfMeasurement);
        if (itemParameters.IsActive)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
        Touch();
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

    /// <summary>
    /// Updates the category identifier associated with the current instance.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the category to assign. Must be a positive number.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="categoryId"/> is less than or equal to zero.</exception>
    public void UpdateCategoryId(long categoryId)
    {
        if (categoryId <= 0)
        {
            throw new ArgumentException("Category ID must be a positive number.", nameof(categoryId));
        }
        CategoryId = categoryId;
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
    /// Updates the item type identifier for the current instance.
    /// </summary>
    /// <param name="itemTypeId">The unique identifier of the item type to assign. Must be a positive number.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="itemTypeId"/> is less than or equal to zero.</exception>
    public void UpdateItemTypeId(long itemTypeId)
    {
        if (itemTypeId <= 0)
        {
            throw new ArgumentException("ItemType Id must be a positive number.", nameof(itemTypeId));
        }
        ItemTypeId = itemTypeId;
    }

    /// <summary>
    /// Updates the unit of measurement used by the current instance.
    /// </summary>
    /// <param name="unitOfMeasurement">The new unit of measurement to assign. Cannot be null, empty, or consist only of white-space characters.</param>
    /// <exception cref="ArgumentException">Thrown if <paramref name="unitOfMeasurement"/> is null, empty, or consists only of white-space characters.</exception>
    public void UpdateUnitOfMeasurement(string unitOfMeasurement)
    {
        if (string.IsNullOrWhiteSpace(unitOfMeasurement))
        {
            throw new ArgumentException("Unit of Measurement cannot be null or empty.", nameof(unitOfMeasurement));
        }
        UnitOfMeasurement = unitOfMeasurement;
    }

    /// <summary>
    /// Deactivates the current instance, setting its active state to false.
    /// </summary>
    /// <remarks>After calling this method, the instance will no longer be considered active. This may affect
    /// its availability for further operations that require an active state.</remarks>
    public void Deactivate() => IsActive = false;

    /// <summary>
    /// Activates the current instance by setting its active state to true.
    /// </summary>
    public void Activate() => IsActive = true;
}
