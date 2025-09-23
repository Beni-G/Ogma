using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Domain.Catalog.Entities;
public class Category : AggregateRoot<long>
{
    public string Name { get; private set; }
    public long? ParentCategoryId { get; set; }

    private readonly List<Category> _subCategories = new();
    public IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();

    public Category(string name, long? parentCategoryId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }
        Name = name;
        ParentCategoryId = parentCategoryId;
    }

    public void AddSubCategory(Category subCategory)
    {
        if (subCategory == null)
        {
            throw new ArgumentNullException(nameof(subCategory));
        }
        if (subCategory.ParentCategoryId != Id)
        {
            throw new InvalidOperationException("Subcategory's parent ID does not match this category's ID.");
        }
        _subCategories.Add(subCategory);
    }

    public IEnumerable<Category> GetAllDescendants()
    {
        foreach (var sub in _subCategories)
        {
            yield return sub;

            foreach (var child in sub.GetAllDescendants())
                yield return child;
        }
    }


    public bool IsRootCategory() => ParentCategoryId == null;
}
