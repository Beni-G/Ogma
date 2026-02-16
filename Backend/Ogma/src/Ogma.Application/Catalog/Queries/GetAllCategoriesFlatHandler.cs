using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Ports;

namespace Ogma.Application.Catalog.Queries;

public class GetAllCategoriesFlatHandler : IRequestHandler<GetAllCategoriesFlatQuery, List<CategoryDto>>
{
    private readonly ICategoryReader _categoryReader;

    public GetAllCategoriesFlatHandler(ICategoryReader categoryReader) => _categoryReader = categoryReader;

    public async Task<List<CategoryDto>> Handle(GetAllCategoriesFlatQuery query, CancellationToken cancellationToken) =>
        (await _categoryReader.GetAllAsync()).ToList();
}
