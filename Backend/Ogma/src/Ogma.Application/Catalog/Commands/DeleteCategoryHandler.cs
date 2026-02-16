using MediatR;
using Ogma.Domain.Catalog.Repositories;

namespace Ogma.Application.Catalog.Commands;
public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    public DeleteCategoryHandler(ICategoryRepository categoryRepository) => _categoryRepository = categoryRepository;

    public async Task Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var existing = await _categoryRepository.GetByIdAsync(command.Id) 
            ?? throw new KeyNotFoundException($"Category with ID {command.Id} was not found.");

        await _categoryRepository.DeleteAsync(existing);
    }
}
