namespace Ogma.Infrastructure.Persistence.Catalog.Models;
public class Item
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string Description { get; set; } = default!;
    public long CategoryId { get; set; }
    public Category Category { get; set; } = default!;
    public decimal ListPriceAmount { get; set; }
    public string ListPriceCurrency { get; set; } = default!;
    public long ItemTypeId { get; set; }
    public ItemType ItemType { get; set; } = default!;
    public string UnitOfMeasurement { get; set; } = default!;
    public bool IsActive { get; set; }

}
