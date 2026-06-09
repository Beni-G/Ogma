using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ogma.Api.Contracts.Catalog;
using Ogma.Api.Extensions;
using Ogma.Application.Catalog.Commands;
using Ogma.Application.Catalog.Queries;

namespace Ogma.Api.Controllers.Catalog;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ItemTypeController : ControllerBase
{
    private readonly IMediator _mediator;
    public ItemTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ItemTypeResponse>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllItemTypesQuery());
        var response = result.Select(it => it.ToResponse()).ToList();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemTypeResponse>> GetById(long id)
    {
        var result = await _mediator.Send(new GetItemTypeByIdQuery(id));
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result.ToResponse());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateItemTypeRequest request)
    {
        var command = new CreateItemTypeCommand(request.Name, request.Description);
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { created.Id }, created.ToResponse());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ItemTypeResponse>> Update(long id, UpdateItemTypeRequest request)
    {
        if(id != request.Id)
        {
            return BadRequest();
        }

        var command = new UpdateItemTypeCommand(request.Id, request.Name, request.Description);
        var updated = await _mediator.Send(command);

        return Ok(updated.ToResponse());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeleteItemTypeCommand(id));
        return NoContent();
    }
}
