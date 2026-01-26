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
public class OrderStatusController : ControllerBase
{
    private readonly IMediator _mediator;
    public OrderStatusController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<OrderStatusResponse>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllOrderStatusesQuery());
        return Ok(result.Select(os => os.ToResponse()).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderStatusResponse>> GetById(long id)
    {
        var result = await _mediator.Send(new GetOrderStatusByIdQuery(id));
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result.ToResponse());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderStatusRequest request)
    {
        var command = new CreateOrderStatusCommand(request.Name, request.Description);
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { created.Id }, created.ToResponse());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OrderStatusResponse>> Update(long id, UpdateOrderStatusRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }
        var command = new UpdateOrderStatusCommand(request.Id, request.Name, request.Description);
        var updated = await _mediator.Send(command);
        return Ok(updated.ToResponse());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeleteOrderStatusCommand(id));
        return NoContent();
    }
}
