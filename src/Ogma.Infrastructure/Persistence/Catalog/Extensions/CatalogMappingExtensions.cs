using Ogma.Domain.Catalog.Parameters;

namespace Ogma.Infrastructure.Persistence.Catalog.Extensions;
public static class CatalogMappingExtensions
{
    /// <summary>
    /// Mapss a Models.Item to a Domain.Catalog.Entities.Item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Domain.Catalog.Entities.Item ToDomain(this Models.Item item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        var itemParameters = new ItemParameters(
            Name: item.Name,
            Code: item.Code,
            Description: item.Description,
            Category: item.Category.ToDomain(),
            ListPrice: new Domain.SharedKernel.ValueObjects.Money(item.ListPriceAmount, item.ListPriceCurrency),
            ItemType: item.ItemType.ToDomain(),
            UnitOfMeasurement: item.UnitOfMeasurement,
            IsActive: item.IsActive
        );

        return Domain.Catalog.Entities.Item.Reconstitute(item.Id, itemParameters);
    }

    /// <summary>
    /// Mapss a Models.Category to a Domain.Catalog.Entities.Category
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Domain.Catalog.Entities.Category ToDomain(this Models.Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }
        var domainCategory = Domain.Catalog.Entities.Category.Reconstitute(category.Id, category.Name, category.ParentCategoryId, category.Path);

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
    public static Domain.Catalog.Entities.ItemType ToDomain(this Models.ItemType itemType)
    {
        if (itemType == null)
        {
            throw new ArgumentNullException(nameof(itemType));
        }
        var domainItemType = Domain.Catalog.Entities.ItemType.Reconstitute(itemType.Id, itemType.Name, itemType.Description);

        return domainItemType;
    }

    /// <summary>
    /// Mapss a Domain.Catalog.Entities.Item to a Models.Item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Models.Item ToModel(this Domain.Catalog.Entities.Item item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        var modelItem = new Models.Item
        {
            Id = item.Id,
            Name = item.Name,
            Code = item.Code,
            Description = item.Description,
            CategoryId = item.Category.Id,
            Category = item.Category.ToModel(),
            ListPriceAmount = item.ListPrice.Amount,
            ListPriceCurrency = item.ListPrice.Currency,
            ItemTypeId = item.ItemType.Id,
            ItemType = item.ItemType.ToModel(),
            UnitOfMeasurement = item.UnitOfMeasurement,
            IsActive = item.IsActive
        };
        return modelItem;
    }

    /// <summary>
    /// Mapss a Domain.Catalog.Entities.Category to a Models.Category
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Models.Category ToModel(this Domain.Catalog.Entities.Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }
        var modelCategory = new Models.Category
        {
            Id = category.Id,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId,
            Path = category.Path
        };
        foreach (var subCategory in category.SubCategories)
        {
            modelCategory.SubCategories.Add(subCategory.ToModel());
        }
        return modelCategory;
    }

    /// <summary>
    /// Maps a Domain.Catalog.Entities.ItemType to a Models.ItemType
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Models.ItemType ToModel(this Domain.Catalog.Entities.ItemType itemType)
    {
        if (itemType == null)
        {
            throw new ArgumentNullException(nameof(itemType));
        }
        var modelItemType = new Models.ItemType
        {
            Id = itemType.Id,
            Name = itemType.Name,
            Description = itemType.Description
        };
        return modelItemType;
    }
}
