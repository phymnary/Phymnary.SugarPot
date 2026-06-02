using Microsoft.CodeAnalysis;

namespace Phymnary.SugarPot.RoslynComponents.Extensions;

public static class AttributeDataExtensions
{
    public static object? GetNamedArgument(this AttributeData attributeData, string name)
    {
        return attributeData
            .NamedArguments.FirstOrDefault(property => property.Key == name)
            .Value.Value;
    }
}
