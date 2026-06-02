using System.Linq.Expressions;

namespace Phymnary.SugarPot.AspNetCore.Extensions;

public static class EnumExtensions
{
    public static bool TryGetExplicit<T>(this T value, out int parsed)
        where T : struct, Enum
    {
        parsed = CompiledLambdaFunc(value);
        return Enum.IsDefined(value);
    }

    private static int CompiledLambdaFunc<TEnum>(TEnum value)
        where TEnum : struct, Enum
    {
        return CachedCast<TEnum>.Cast(value);
    }

    private static class CachedCast<T>
        where T : struct, Enum
    {
        public static readonly Func<T, int> Cast = GenerateFunc<T>();
    }

    private static Func<TEnum, int> GenerateFunc<TEnum>()
        where TEnum : struct, Enum
    {
        var inputParameter = Expression.Parameter(typeof(TEnum));

        var body = Expression.Convert(inputParameter, typeof(int));
        var lambda = Expression.Lambda<Func<TEnum, int>>(body, inputParameter);
        var func = lambda.Compile();

        return func;
    }
}
