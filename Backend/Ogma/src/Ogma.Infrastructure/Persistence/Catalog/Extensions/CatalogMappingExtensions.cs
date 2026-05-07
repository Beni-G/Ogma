using Ogma.Application.Catalog.Dtos;
using Ogma.Application.SharedKernel.Dtos;
using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Parameters;
using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.SharedKernel.ValueObjects;
using Ogma.Infrastructure.Persistence.SharedKernel.BaseTypes;
using Ogma.Infrastructure.Persistence.SharedKernel.Extensions;

namespace Ogma.Infrastructure.Persistence.Catalog.Extensions;
public static class CatalogMappingExtensions
{
    #region ToDto
    /// <summary>
    /// Creates a new <see cref="ItemDto"/> instance that represents the specified <see cref="Models.Item"/>.
    /// </summary>
    /// <param name="item">The item to convert to a data transfer object. Cannot be <see langword="null"/>.</param>
    /// <returns>An <see cref="ItemDto"/> containing the data from the specified <paramref name="item"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is <see langword="null"/>.</exception>
    public static ItemDto ToDto(this Models.Item item, IEnumerable<CategoryDto>? categoryAncestors = null, IEnumerable<CategoryDto>? categorySubCategories = null) => 
        item == null 
        ? throw new ArgumentNullException(nameof(item)) 
        : new ItemDto(
            item.Id, 
            item.Name, 
            item.Code, 
            item.Description, 
            item.CategoryId,
            item.Category.ToDto(categoryAncestors, categorySubCategories), 
            new MoneyDto(item.ListPriceAmount, item.ListPriceCurrency), 
            item.ItemTypeId,
            item.ItemType.ToDto(), 
            item.UnitOfMeasurement, 
            item.IsActive);

    /// <summary>
    /// Creates a new instance of <see cref="ItemDto"/> based on the specified item, optionally including category
    /// ancestor and subcategory information.
    /// </summary>
    /// <param name="item">The source <see cref="ItemDto"/> to convert. Cannot be null.</param>
    /// <param name="categoryAncestors">An optional collection of ancestor categories to associate with the item's category. May be null if no ancestor
    /// information is needed.</param>
    /// <param name="categorySubCategories">An optional collection of subcategories to associate with the item's category. May be null if no subcategory
    /// information is needed.</param>
    /// <returns>A new <see cref="ItemDto"/> instance containing the data from <paramref name="item"/>, with category information
    /// populated according to the provided ancestors and subcategories.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is null.</exception>
    public static ItemDto ToDto(this ItemDto item, IEnumerable<CategoryDto>? categoryAncestors = null, IEnumerable<CategoryDto>? categorySubCategories = null) =>
        item == null
        ? throw new ArgumentNullException(nameof(item))
        : new ItemDto(
            item.Id,
            item.Name,
            item.Code,
            item.Description,
            item.CategoryId,
            item.Category?.ToDto(categoryAncestors, categorySubCategories),
            item.ListPrice,
            item.ItemTypeId,
            item.ItemType,
            item.UnitOfMeasurement,
            item.IsActive);

    /// <summary>
    /// Converts a <see cref="Models.Category"/> instance to its corresponding <see cref="CategoryDto"/> representation.
    /// </summary>
    /// <param name="category">The <see cref="Models.Category"/> object to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>A <see cref="CategoryDto"/> containing the data from the specified <see cref="Models.Category"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="category"/> is <see langword="null"/>.</exception>
    public static CategoryDto ToDto(this Models.Category category, IEnumerable<CategoryDto>? ancestors = null, IEnumerable<CategoryDto>? subCategories = null) => 
        category == null 
        ? throw new ArgumentNullException(nameof(category)) 
        : new CategoryDto(
            category.Id, 
            category.Name, 
            category.ParentCategoryId, 
            category.Path, 
            ancestors?.ToList() ?? [], 
            subCategories?.ToList() ?? []
        );

    /// <summary>
    /// Converts a <see cref="Models.Category"/> instance to a <see cref="CategoryDto"/>, recursively converting all subcategories as well.
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public static CategoryDto ToDtoRecursive(this Models.Category category)
    {
        var subCategoryDtos = category.SubCategories?
            .Select(sc => sc.ToDtoRecursive()) // Recursive call
            .ToList();

        return category.ToDto(subCategories: subCategoryDtos);
    }

    /// <summary>
    /// Creates a new <see cref="CategoryDto"/> instance with the specified category's data, optionally including
    /// ancestor and subcategory information.
    /// </summary>
    /// <param name="category">The source <see cref="CategoryDto"/> whose data is used to create the new instance. Cannot be null.</param>
    /// <param name="ancestors">An optional collection of ancestor categories to associate with the new instance. If null, an empty list is
    /// used.</param>
    /// <param name="subCategories">An optional collection of subcategories to associate with the new instance. If null, an empty list is used.</param>
    /// <returns>A new <see cref="CategoryDto"/> containing the data from <paramref name="category"/>, with the specified
    /// ancestors and subcategories.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="category"/> is null.</exception>
    public static CategoryDto ToDto(this CategoryDto category, IEnumerable<CategoryDto>? ancestors = null, IEnumerable<CategoryDto>? subCategories = null) =>
        category == null
        ? throw new ArgumentNullException(nameof(category))
        : new CategoryDto(
            category.Id,
            category.Name,
            category.ParentCategoryId,
            category.Path,
            ancestors?.ToList() ?? [],
            subCategories?.ToList() ?? []
        );

    /// <summary>
    /// Converts an <see cref="Models.ItemType"/> instance to its corresponding <see cref="ItemTypeDto"/>
    /// representation.
    /// </summary>
    /// <param name="itemType">The <see cref="Models.ItemType"/> object to convert. Cannot be <see langword="null"/>.</param>
    /// <returns>An <see cref="ItemTypeDto"/> containing the identifier, name, and description from the specified <paramref
    /// name="itemType"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="itemType"/> is <see langword="null"/>.</exception>
    public static ItemTypeDto ToDto(this Models.ItemType itemType) =>
        itemType == null 
        ? throw new ArgumentNullException(nameof(itemType)) 
        : new ItemTypeDto(itemType.Id, itemType.Name, itemType.Description);
    #endregion

    #region ToDomain
    /// <summary>
    /// Mapss a Models.Item to a Domain.Catalog.Entities.Item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Item ToDomain(this Models.Item item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        var itemParameters = new ItemParameters(
            Name: item.Name,
            Code: item.Code,
            Description: item.Description,
            CategoryId: item.CategoryId,
            ListPrice: new Money(item.ListPriceAmount, item.ListPriceCurrency),
            ItemTypeId: item.ItemTypeId,
            UnitOfMeasurement: item.UnitOfMeasurement,
            IsActive: item.IsActive
        );

        return Item.Reconstitute(item.Id, itemParameters, new EntityMetadata(item.CreatedAt, item.UpdatedAt, item.Version));
    }

    /// <summary>
    /// Mapss a Models.Category to a Domain.Catalog.Entities.Category
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Category ToDomain(this Models.Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }
        var domainCategory = Category.Reconstitute(
            category.Id, 
            category.Name, 
            new EntityMetadata(category.CreatedAt, category.UpdatedAt, category.Version), 
            category.ParentCategoryId, 
            category.Path);

        foreach (var subCategory in category.SubCategories)
        {
            domainCategory.AddSubCategory(subCategory.ToDomain());
        }
        return domainCategory;
    }

    /// <summary>
    /// Mapss a Models.ItemType to a Domain.Catalog.Entities.ItemType
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static ItemType ToDomain(this Models.ItemType itemType)
    {
        if (itemType == null)
        {
            throw new ArgumentNullException(nameof(itemType));
        }
        var domainItemType = ItemType.Reconstitute(itemType.Id, itemType.Name, itemType.Description, new EntityMetadata(itemType.CreatedAt, itemType.UpdatedAt, itemType.Version));

        return domainItemType;
    }

    #endregion

    #region ToModel

    /// <summary>
    /// Mapss a Domain.Catalog.Entities.Item to a Models.Item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Models.Item ToModel(this Item item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        var modelItem = new Models.Item
        {
            Name = item.Name,
            Code = item.Code,
            Description = item.Description,
            CategoryId = item.CategoryId,
            ListPriceAmount = item.ListPrice.Amount,
            ListPriceCurrency = item.ListPrice.Currency,
            ItemTypeId = item.ItemTypeId,
            UnitOfMeasurement = item.UnitOfMeasurement,
            IsActive = item.IsActive
        };
        item.MapBaseProperties(modelItem);
        return modelItem;
    }

    /// <summary>
    /// Mapss a Domain.Catalog.Entities.Category to a Models.Category
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Models.Category ToModel(this Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }
        var modelCategory = new Models.Category
        {
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId,
            Path = category.Path
        };
        category.MapBaseProperties(modelCategory);
        return modelCategory;
    }

    /// <summary>
    /// Maps a Domain.Catalog.Entities.ItemType to a Models.ItemType
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Models.ItemType ToModel(this ItemType itemType)
    {
        if (itemType == null)
        {
            throw new ArgumentNullException(nameof(itemType));
        }
        var modelItemType = new Models.ItemType
        {
            Name = itemType.Name,
            Description = itemType.Description
        };
        itemType.MapBaseProperties(modelItemType);
        return modelItemType;
    }

    #endregion
}
