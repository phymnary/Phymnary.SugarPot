using System.Linq.Expressions;
using System.Reflection;
using Phymnary.SugarPot.AspNetCore.Entities;
using Phymnary.SugarPot.AspNetCore.Exceptions;

namespace Phymnary.SugarPot.AspNetCore;

public class EntitySchemaLookup
{
    /// <summary>
    /// the Value is Func<TEntity, TKey> to lookup key because an entity can have different key types, and we want to avoid boxing/unboxing when the key is a value type. We will compile the expression tree to get the Func<TEntity, TKey> at runtime.
    /// </summary>
    public readonly Dictionary<Type, object> _keyLookups = [];

    public void Add<TEntity>()
        where TEntity : class, IEntity
    {
        var type = typeof(TEntity);
        MethodInfo methodInfo =
            type.GetMethod("GetKey", Type.EmptyTypes)
            ?? throw new DomainNotImplementedException(
                "Entity must implement IHasKey{T} and GetKey method"
            );

        ParameterExpression param = Expression.Parameter(type, "entity");
        MethodCallExpression methodCall = Expression.Call(param, methodInfo);
        LambdaExpression lambda = Expression.Lambda(methodCall, param);
        var call = lambda.Compile();

        _keyLookups[typeof(TEntity)] = lambda.Compile();
    }
}
