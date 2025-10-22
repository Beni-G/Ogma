using Ogma.Domain.Catalog.Entities;

namespace Ogma.Domain.Catalog.Services;
public interface ICategoryDomainService
{
    /// <summary>
    /// Computes the path for a category based on its parent and validates max depth.
    /// Throws if rules are violated (max depth exceeded or circular reference).
    /// </summary>
    string ComputePath(Category category, Category? parent);
}
