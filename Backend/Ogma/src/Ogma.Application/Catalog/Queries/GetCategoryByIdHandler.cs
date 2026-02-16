using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Ports;

namespace Ogma.Application.Catalog.Queries;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly ICategoryReader _categoryReader;

    public GetCategoryByIdHandler(ICategoryReader categoryReader) => _categoryReader = categoryReader;

    public async Task<CategoryDto> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        return await _categoryReader.GetByIdAsync(query.Id)
            ?? throw new KeyNotFoundException($"Category with ID {query.Id} not found.");
    }
}
