using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ogma.Api.Contracts.Catalog;
using Ogma.Api.Extensions;
using Ogma.Application.Catalog.Commands;
using Ogma.Application.Catalog.Queries;

namespace Ogma.Api.Controllers.Catalog;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetAllFlat()
    {
        var result = await _mediator.Send(new GetAllCategoriesFlatQuery());
        var response = result.Select(c => c.ToResponse()).ToList();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponse>> GetById(long id)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery(id));
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result.ToResponse());
    }

    [HttpGet("tree")]
    public async Task<ActionResult<List<CategoryResponse>>> GetAllTree()
    {
        var result = await _mediator.Send(new GetAllCategoriesTreeQuery());
        var response = result.Select(c => c.ToResponse()).ToList();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> Create(CreateCategoryRequest request)
    {
        var command = new CreateCategoryCommand(request.Name, request.ParentCategoryId);
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { created.Id }, created.ToResponse());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryResponse>> Update(long id, UpdateCategoryRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        var command = new UpdateCategoryCommand(request.Id, request.Name, request.ParentCategoryId);
        var updated = await _mediator.Send(command);

        return Ok(updated.ToResponse());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id));
        return NoContent();
    }
}
