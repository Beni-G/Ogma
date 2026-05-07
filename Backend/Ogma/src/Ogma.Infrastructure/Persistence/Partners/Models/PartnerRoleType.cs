using Ogma.Infrastructure.Persistence.SharedKernel.BaseTypes;

namespace Ogma.Infrastructure.Persistence.Partners.Models;

public class PartnerRoleType : Entity
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Color { get; set; }
}
