namespace AspNetCore.Boilerplate.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> AppendIfNotNull<T>(this IEnumerable<T> source, T? value)
        where T : notnull
    {
        if (value is null)
            return source;

        return [.. source, value];
    }

    public static string JoinAsString<T>(this IEnumerable<T> source, string separator)
    {
        return string.Join(separator, source);
    }

    public static string JoinAsString<T>(this IEnumerable<T> source, char separator)
    {
        return string.Join(separator, source);
    }

    public static int IndexOf<T>(this T[] source, T value)
    {
        return Array.IndexOf(source, value);
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source)
        where T : class
    {
        return source.Where(it => it is not null)!;
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source)
        where T : struct
    {
        return source.Where(it => it is not null).Select(it => it!.Value);
    }
}
