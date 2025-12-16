using MediatR;
using Ogma.Application.Partners.Dtos;
using Ogma.Application.Partners.Extensions;
using Ogma.Application.SharedKernel.Extensions;
using Ogma.Domain.Partners.Entities;
using Ogma.Domain.Partners.Repositories;

namespace Ogma.Application.Partners.Commands;

public class CreatePartnerHandler : IRequestHandler<CreatePartnerCommand, PartnerDto>
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly IPartnerRoleTypeRepository _partnerRoleTypeRepository;

    public CreatePartnerHandler(IPartnerRepository partnerRepository, IPartnerRoleTypeRepository partnerRoleTypeRepository)
    {
        _partnerRepository = partnerRepository;
        _partnerRoleTypeRepository = partnerRoleTypeRepository;
    }

    public async Task<PartnerDto> Handle(CreatePartnerCommand command, CancellationToken cancellationToken)
    {
        var partnerRole = await _partnerRoleTypeRepository.GetByIdAsync(command.RoleId) ?? throw new InvalidOperationException($"Partner role with ID {command.RoleId} not found.");

        var partner = command.IsNaturalPerson
            ? Partner.CreateIndividual(
                command.IndividualName!.ToDomain(),
                command.Identifier.ToDomain(),
                partnerRole,
                command.HQAddress!.ToDomain())
            : Partner.CreateLegalEntity(
                command.CompanyName!,
                command.Identifier.ToDomain(),
                partnerRole,
                command.HQAddress!.ToDomain());


        var created = await _partnerRepository.AddAsync(partner);
        return created.ToDto();
    }
}
