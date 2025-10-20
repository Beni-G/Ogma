using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ogma.Api.Contracts.Catalog;
using Ogma.Api.Extensions;
using Ogma.Application.Catalog.Commands;
using Ogma.Application.Catalog.Queries;

namespace Ogma.Api.Controllers.Catalog;
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryWithDescendantsResponse>>> GetAllFlat()
    {
        var result = await _mediator.Send(new GetAllCategoriesFlatQuery());
        var response = result.Select(c => c.ToResponseWithDescendants()).ToList();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryWithDescendantsResponse>> GetById(long id)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery(id));
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result.ToResponseWithDescendants());
    }

    [HttpGet("tree")]
    public async Task<ActionResult<List<CategoryWithDescendantsResponse>>> GetAllTree()
    {
        var result = await _mediator.Send(new GetAllCategoriesTreeQuery());
        var response = result.Select(c => c.ToResponseWithDescendants()).ToList();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryWithDescendantsResponse>> CreateCategory(CreateCategoryRequest request)
    {
        var command = new CreateCategoryCommand(request.Name, request.ParentCategoryId);
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { created.Id }, created.ToResponseWithDescendants());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryWithDescendantsResponse>> UpdateCategory(long id, UpdateCategoryRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }

        var command = new UpdateCategoryCommand(request.Id, request.Name, request.ParentCategoryId);
        var updated = await _mediator.Send(command);

        return Ok(updated.ToResponseWithDescendants());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItemType(long id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id));
        return NoContent();
    }
}
