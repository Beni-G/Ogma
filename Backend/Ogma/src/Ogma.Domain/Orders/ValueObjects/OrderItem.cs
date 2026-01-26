using Ogma.Domain.SharedKernel.BaseTypes;

namespace Ogma.Domain.Orders.ValueObjects;

public class OrderItem : ValueObject
{
    public long ItemId { get; }
    public string ItemName { get; }
    public string ItemCode { get; }

    public OrderItem(long itemId, string itemName, string itemCode)
    {
        if (itemId <= 0)
        {
            throw new ArgumentException("ItemId must be greater than zero.", nameof(itemId));
        }
        if (string.IsNullOrWhiteSpace(itemName))
        {
            throw new ArgumentException("ItemName cannot be null or empty.", nameof(itemName));
        }
        if (string.IsNullOrWhiteSpace(itemCode))
        {
            throw new ArgumentException("ItemCode cannot be null or empty.", nameof(itemCode));
        }

        ItemId = itemId;
        ItemName = itemName;
        ItemCode = itemCode;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ItemId;
        yield return ItemName;
        yield return ItemCode;
    }

    public override string ToString() => $"{ItemName} (Code: {ItemCode}, ID: {ItemId})";
}
