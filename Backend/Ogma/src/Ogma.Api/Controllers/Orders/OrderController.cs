using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ogma.Api.Contracts.Orders;
using Ogma.Api.Extensions;
using Ogma.Application.Orders.Commands;
using Ogma.Application.Orders.Dtos;
using Ogma.Application.Orders.Queries;
using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Api.Controllers.Orders;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetAllOrders()
    {
        var result = await _mediator.Send(new GetAllOrdersQuery());
        return Ok(result.Select(o => o.ToResponse()).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetOrderById(long id)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(id));
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result.ToResponse());
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var command = new CreateOrderCommand(
            request.PartnerId,
            request.OrderNumber,
            request.OrderDate,
            request.OrderTypeId,
            request.OrderStatusId,
            request.AdditionalInformation,
            request.OrderLines.Select(ol =>
                new CreateOrderLineDto(
                    ol.ItemId,
                    ol.OrderedQuantity,
                    new MoneyDto(ol.Price.Amount, ol.Price.Currency),
                    ol.ExchangeRate != null
                        ? new ExchangeRateDto(ol.ExchangeRate.BaseCurrency, ol.ExchangeRate.TargetCurrency, ol.ExchangeRate.Rate)
                        : default,
                    ol.AdditionalInformation
                )
            ).ToList()
        );

        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOrderById), new { id = created.Id }, created.ToResponse());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OrderResponse>> UpdateOrder(long id, UpdateOrderRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest();
        }
        var command = new UpdateOrderCommand(
            request.Id,
            request.PartnerId,
            request.OrderNumber,
            request.OrderDate,
            request.OrderTypeId,
            request.OrderStatusId,
            request.AdditionalInformation!,
            request.OrderLines.Select(ol =>
                new UpdateOrderLineDto(
                    ol.Id,
                    ol.ItemId,
                    ol.OrderedQuantity,
                    ol.CancelledQuantity,
                    ol.FullfilledQuantity,
                    new MoneyDto(ol.Price.Amount, ol.Price.Currency),
                    ol.ExchangeRate != null
                        ? new ExchangeRateDto(ol.ExchangeRate.BaseCurrency, ol.ExchangeRate.TargetCurrency, ol.ExchangeRate.Rate)
                        : default,
                    ol.AdditionalInformation
                )
            ).ToList()
        );
        var updated = await _mediator.Send(command);
        return Ok(updated.ToResponse());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeleteOrderCommand(id));
        return NoContent();
    }
}
