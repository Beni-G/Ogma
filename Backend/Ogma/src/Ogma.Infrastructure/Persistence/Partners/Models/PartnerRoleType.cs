namespace Ogma.Infrastructure.Persistence.Partners.Models;

public class PartnerRoleType
{
    public long Id { get; set; }
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Color { get; set; }
}
