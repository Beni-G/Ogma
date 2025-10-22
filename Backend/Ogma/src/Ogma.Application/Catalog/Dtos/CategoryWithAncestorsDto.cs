namespace Ogma.Application.Catalog.Dtos;
public record CategoryWithAncestorsDto(long Id, string Name, long? ParentCategoryId, string? Path, IReadOnlyCollection<CategoryDto> Ancestors);
