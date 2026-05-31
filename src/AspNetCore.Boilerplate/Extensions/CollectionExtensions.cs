namespace AspNetCore.Boilerplate.Extensions;

public static class CollectionExtensions
{
    public static void AddIfNotExisted<T>(this ICollection<T> collection, T item)
    {
        if (!collection.Contains(item))
        {
            collection.Add(item);
        }
    }
}
