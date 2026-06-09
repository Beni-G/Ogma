using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ogma.Api.Contracts.Partners;
using Ogma.Api.Extensions;
using Ogma.Application.Partners.Commands;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Queries;
using Ogma.Application.SharedKernel.Dtos;

namespace Ogma.Api.Controllers.Partners;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PartnerController : ControllerBase
{
    private readonly IMediator _mediator;

    public PartnerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<PartnerResponse>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllPartnersQuery());
        var response = result.Select(p => p.ToResponse()).ToList();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PartnerResponse>> GetById(long id)
    {
        var result = await _mediator.Send(new GetPartnerByIdQuery(id));
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result.ToResponse());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePartnerRequest request)
    {
        var individualNameDto = request.IndividualName != null
            ? new PersonNameDto(request.IndividualName.FirstName, request.IndividualName.LastName)
            : null;

        var command = new CreatePartnerCommand(
            individualNameDto,
            request.CompanyName,
            request.IsNaturalPerson,
            request.HQAddress != null
                ? new AddressDto(
                    request.HQAddress.Street,
                    request.HQAddress.Number,
                    request.HQAddress.City,
                    request.HQAddress.Region,
                    request.HQAddress.PostalCode,
                    request.HQAddress.CountryCode,
                    request.HQAddress.Building,
                    request.HQAddress.StairCase,
                    request.HQAddress.Floor,
                    request.HQAddress.Apartment)
                : null,
            new CreatePartnerIdentifierDto(
                request.Identifier.Type,
                request.Identifier.Value,
                request.Identifier.ValidityPeriod != null
                    ? new PeriodDto(request.Identifier.ValidityPeriod.Start, request.Identifier.ValidityPeriod.End)
                    : null,
                request.Identifier.IsPrimary),
            request.RoleId);

        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { created.Id }, created.ToResponse());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, UpdatePartnerRequest request)
    {
        var individualNameDto = request.IndividualName != null
            ? new PersonNameDto(request.IndividualName.FirstName, request.IndividualName.LastName)
            : null;
        var command = new UpdatePartnerCommand(
            id,
            individualNameDto,
            request.CompanyName,
            request.IsNaturalPerson,
            request.IsActive,
            request.DisplayName,
            request.HQAddress != null
                ? new AddressDto(
                    request.HQAddress.Street,
                    request.HQAddress.Number,
                    request.HQAddress.City,
                    request.HQAddress.Region,
                    request.HQAddress.PostalCode,
                    request.HQAddress.CountryCode,
                    request.HQAddress.Building,
                    request.HQAddress.StairCase,
                    request.HQAddress.Floor,
                    request.HQAddress.Apartment)
                : null,
            request.Identifiers.Select(idt => new PartnerIdentifierDto(
                idt.Id,
                idt.Type,
                idt.Value,
                idt.ValidityPeriod != null
                    ? new PeriodDto(idt.ValidityPeriod.Start, idt.ValidityPeriod.End)
                    : null,
                idt.IsPrimary)).ToList(),
            request.RoleIds,
            request.BankAccounts.Select(ba => new PartnerBankAccountDto(
                ba.Id,
                new BankAccountDto(
                    ba.BankAccount.Bank,
                    ba.BankAccount.Iban,
                    ba.BankAccount.Currency,
                    ba.BankAccount.Bic),
                ba.IsDefault)).ToList(),
            request.Contacts.Select(c => new PartnerContactDto(
                c.Id,
                new PersonNameDto(c.Name.FirstName, c.Name.LastName),
                c.Email,
                c.Phone,
                c.Mobile,
                c.Title,
                c.JobTitle,
                c.IsPrimary)).ToList());
        var updated = await _mediator.Send(command);
        return Ok(updated.ToResponse());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _mediator.Send(new DeletePartnerCommand(id));
        return NoContent();
    }
}
