using Ogma.Domain.Catalog.Entities;
using Ogma.Domain.Catalog.Services;

namespace Ogma.Application.Catalog.Services;
public class CategoryDomainService : ICategoryDomainService
{
    public string ComputePath(Category category, Category? parent)
    {
        if (parent == null)
        {
            return string.Empty;
        }

        if (parent.Depth >= Category.MaxDepth)
        {
            throw new InvalidOperationException(
                $"Cannot create subcategory beyond maximum depth of {Category.MaxDepth}.");
        }

        // Prevent circular references
        if (!string.IsNullOrEmpty(parent.GetFullPath()) &&
            parent.GetFullPath().Split('/').Contains(category.Id.ToString()))
        {
            throw new InvalidOperationException("Cannot set a descendant as parent category.");
        }

        return parent.GetFullPath();
    }
}
