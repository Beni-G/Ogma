namespace Ogma.Application.Catalog.Dtos;
public record CategoryWithDescendantsDto(long Id, string Name, long? ParentCategoryId, string? Path, IReadOnlyCollection<CategoryWithDescendantsDto> SubCategories);
