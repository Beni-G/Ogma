namespace Ogma.Domain.SharedKernel.Extentions;
public static class CollectionExtensions
{
    public static void ReplaceWith<T>(this ICollection<T> collection, IEnumerable<T> source)
    {
        collection.Clear();
        foreach (var item in source)
        {
            collection.Add(item);
        }
    }
}
