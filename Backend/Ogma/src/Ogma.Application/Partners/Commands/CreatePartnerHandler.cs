using MediatR;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Extensions;
using Ogma.Application.Partners.Ports;
using Ogma.Application.SharedKernel.Extensions;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.Partners.Repositories;

namespace Ogma.Application.Partners.Commands;

public class CreatePartnerHandler : IRequestHandler<CreatePartnerCommand, PartnerDto>
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly IPartnerReader _partnerReader;
    private readonly IPartnerRoleTypeReader _partnerRoleTypeReader;

    public CreatePartnerHandler(IPartnerRepository partnerRepository, IPartnerReader partnerReader, IPartnerRoleTypeReader partnerRoleTypeReader)
    {
        _partnerRepository = partnerRepository;
        _partnerReader = partnerReader;
        _partnerRoleTypeReader = partnerRoleTypeReader;
    }

    public async Task<PartnerDto> Handle(CreatePartnerCommand command, CancellationToken cancellationToken)
    {
        var partnerRole = await _partnerRoleTypeReader.GetByIdAsync(command.RoleId) 
            ?? throw new KeyNotFoundException($"Partner role with ID {command.RoleId} not found.");

        var partner = command.IsNaturalPerson
            ? Partner.CreateIndividual(
                command.IndividualName!.ToDomain(),
                PartnerIdentifier.Create(
                    command.Identifier.Type, 
                    command.Identifier.Value, 
                    command.Identifier.ValidityPeriod?.ToDomain() , 
                    command.Identifier.IsPrimary),
                command.RoleId,
                command.HQAddress!.ToDomain())
            : Partner.CreateLegalEntity(
                command.CompanyName!,
                PartnerIdentifier.Create(
                    command.Identifier.Type,
                    command.Identifier.Value,
                    command.Identifier.ValidityPeriod?.ToDomain(),
                    command.Identifier.IsPrimary),
                command.RoleId,
                command.HQAddress!.ToDomain());


        var created = await _partnerRepository.AddAsync(partner);
        return await _partnerReader.GetByIdAsync(created.Id)
            ?? throw new InvalidOperationException($"Failed to retrieve created Partner with ID {created.Id}.");
    }
}
