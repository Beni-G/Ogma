namespace Ogma.Infrastructure.Persistence.Partners.Models;

public class PartnerContact
{
    public long Id { get; set; }
    public long PartnerId { get; set; }
    public Partner Partner { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Title { get; set; }
    public string? JobTitle { get; set; }
    public bool IsPrimary { get; set; }
}
