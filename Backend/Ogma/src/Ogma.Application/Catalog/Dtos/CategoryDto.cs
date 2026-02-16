namespace Ogma.Application.Catalog.Dtos;
public record CategoryDto(
    long Id, 
    string Name, 
    long? ParentCategoryId = default, 
    string? Path = default, 
    IReadOnlyCollection<CategoryDto>? Ancestors = null,
    IReadOnlyCollection<CategoryDto>? SubCategories = null
);

