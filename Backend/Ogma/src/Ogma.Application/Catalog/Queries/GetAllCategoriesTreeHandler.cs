using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.Catalog.Ports;

namespace Ogma.Application.Catalog.Queries;

public class GetAllCategoriesTreeHandler : IRequestHandler<GetAllCategoriesTreeQuery, List<CategoryDto>>
{
    private readonly ICategoryReader _categoryReader;

    public GetAllCategoriesTreeHandler(ICategoryReader categoryReader) => _categoryReader = categoryReader;

    public async Task<List<CategoryDto>> Handle(GetAllCategoriesTreeQuery query, CancellationToken cancellationToken) =>
        (await _categoryReader.GetAllCategoriesTreeAsync()).ToList();

}
