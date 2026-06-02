using System.Reflection;

namespace Phymnary.SugarPot.AspNetCore.Extensions;

public static class AttributeProviderExtensions
{
    public static bool HasAttribute<TAttribute>(this ICustomAttributeProvider type)
        where TAttribute : Attribute
    {
        return type.GetCustomAttributes(typeof(TAttribute), false).Length > 0;
    }
}
