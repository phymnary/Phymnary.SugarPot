namespace AspNetCore.Boilerplate.EntityFrameworkCore.Extensions;

public static class GuidExtensions
{
    public static Guid? NullIfEmpty(this Guid? value)
    {
        return value == Guid.Empty ? null : value;
    }

    public static bool TryGetValuable(this Guid? value, out Guid result)
    {
        if (value is null || value == Guid.Empty)
        {
            result = Guid.Empty;
            return false;
        }

        result = value.Value;
        return true;
    }
}
