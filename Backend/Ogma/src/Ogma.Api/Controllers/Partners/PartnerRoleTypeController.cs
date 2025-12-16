using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ogma.Api.Contracts.Partners;
using Ogma.Api.Extensions;
using Ogma.Application.Partners.Commands;
using Ogma.Application.Partners.Queries;

namespace Ogma.Api.Controllers.Partners;

[Route("api/[controller]")]
[ApiController]
public class PartnerRoleTypeController : ControllerBase
{
    private readonly IMediator _mediator;

    public PartnerRoleTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<PartnerRoleTypeResponse>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllPartnerRoleTypesQuery());
        var response = result.Select(prt => prt.ToResponse()).ToList();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PartnerRoleTypeResponse>> GetById(long id)
    {
        var result = await _mediator.Send(new GetPartnerRoleTypeByIdQuery(id));
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result.ToResponse());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePartnerRoleTypeRequest request)
    {
        var command = new CreatePartnerRoleTypeCommand(request.Code, request.Name, request.Color);
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { created.Id }, created.ToResponse());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PartnerRoleTypeResponse>> Update(long id, UpdatePartnerRoleTypeRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        var command = new UpdatePartnerRoleTypeCommand(request.Id, request.Code, request.Name, request.Color);
        var updated = await _mediator.Send(command);

        return Ok(updated.ToResponse());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeletePartnerRoleTypeCommand(id));
        return NoContent();
    }
}
