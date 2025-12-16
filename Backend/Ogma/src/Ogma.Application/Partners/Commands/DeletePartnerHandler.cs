using MediatR;
using Ogma.Domain.Partners.Repositories;

namespace Ogma.Application.Partners.Commands;

public class DeletePartnerHandler : IRequestHandler<DeletePartnerCommand>
{
    private readonly IPartnerRepository _partnerRepository;

    public DeletePartnerHandler(IPartnerRepository partnerRepository) => _partnerRepository = partnerRepository;
    public async Task Handle(DeletePartnerCommand command, CancellationToken cancellationToken)
    {
        var existingPartner = await _partnerRepository.GetByIdAsync(command.Id)
            ?? throw new KeyNotFoundException($"Partner with ID {command.Id} was not found.");

        await _partnerRepository.DeleteAsync(existingPartner);
    }
}
