using MediatR;
using Ogma.Application.Catalog.Dtos;
using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Application.Catalog.Commands;
public record CreateItemCommand(string Name,
    string Code,
    long CategoryId,
    MoneyDto ListPrice,
    long ItemTypeId,
    string UnitOfMeasurement,
    bool IsActive,
    string Description = "") : IRequest<ItemDto>;

