using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ogma.Api.Contracts.Catalog;
using Ogma.Api.Extensions;
using Ogma.Application.Catalog.Commands;
using Ogma.Application.Catalog.Queries;
using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Api.Controllers.Catalog;
[Route("api/[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ItemResponse>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllItemsQuery());
        var response = result.Select(i => i.ToResponse()).ToList();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemResponse>> GetById(long id)
    {
        var query = new GetItemByIdQuery(id);
        var item = await _mediator.Send(query);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<ItemResponse>> Create(CreateItemRequest request)
    {
        var command = new CreateItemCommand(request.Name,
            request.Code,
            request.CategoryId,
            new MoneyDto(request.ListPrice.Amount, request.ListPrice.Currency),
            request.ItemTypeId,
            request.UnitOfMeasurement,
            request.IsActive,
            request.Description);
        var created = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { created.Id }, created.ToResponse());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ItemResponse>> Update(long id, UpdateItemRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest();

        }
        var command = new UpdateItemCommand(request.Id,
            request.Name,
            request.Code,
            request.CategoryId,
            new MoneyDto(request.ListPrice.Amount, request.ListPrice.Currency),
            request.ItemTypeId,
            request.UnitOfMeasurement,
            request.IsActive,
            request.Description);

        var updated = await _mediator.Send(command);

        return Ok(updated.ToResponse());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeleteItemCommand(id));
        return NoContent();
    }
}
