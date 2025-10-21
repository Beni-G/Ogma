namespace Ogma.Application.Catalog.Dtos;
public record CategoryDto(long Id, string Name, long? ParentCategoryId, string? Path);

