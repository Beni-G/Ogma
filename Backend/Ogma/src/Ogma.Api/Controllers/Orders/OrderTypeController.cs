using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ogma.Api.Contracts.Orders;
using Ogma.Api.Extensions;
using Ogma.Application.Orders.Commands;
using Ogma.Application.Orders.Queries;

namespace Ogma.Api.Controllers.Orders;

[Route("api/[controller]")]
[ApiController]
public class OrderTypeController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderTypeController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<OrderTypeResponse>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllOrderTypesQuery());
        return Ok(result.Select(ot => ot.ToResponse()).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderTypeResponse>> GetById(long id)
    {
        var result = await _mediator.Send(new GetOrderTypeByIdQuery(id));
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result.ToResponse());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderTypeRequest request)
    {
        var command = new CreateOrderTypeCommand(request.Code, request.Description);
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { created.Id }, created.ToResponse());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OrderTypeResponse>> Update(long id, UpdateOrderTypeRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }
        var command = new UpdateOrderTypeCommand(request.Id, request.Code, request.Description);
        var updated = await _mediator.Send(command);
        return Ok(updated.ToResponse());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeleteOrderTypeCommand(id));
        return NoContent();
    }
}
