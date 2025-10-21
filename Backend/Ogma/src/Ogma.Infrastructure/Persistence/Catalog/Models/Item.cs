namespace Ogma.Infrastructure.Persistence.Catalog.Models;
public class Item
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public long CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public decimal ListPriceAmount { get; set; }
    public string ListPriceCurrency { get; set; }
    public long ItemTypeId { get; set; }
    public ItemType ItemType { get; set; } = null!;
    public string UnitOfMeasurement { get; set; } 
    public bool IsActive { get; set; }

}
