using System.Linq.Expressions;
using System.Reflection;
using Phymnary.SugarPot.AspNetCore.Entities;
using Phymnary.SugarPot.AspNetCore.Exceptions;

namespace Phymnary.SugarPot.AspNetCore;

public class EntitySchemaLookup
{
    private readonly Dictionary<Type, Func<IEntity, object>> _keyLookups = [];

    public void Add<TEntity>()
        where TEntity : class, IEntity
    {
        var type = typeof(TEntity);
        MethodInfo methodInfo =
            type.GetMethod("GetKey", Type.EmptyTypes)
            ?? throw new DomainNotImplementedException(
                "Entity must implement IHasKey{T} and GetKey method"
            );
        ParameterExpression param = Expression.Parameter(typeof(IEntity), "entity");
        MethodCallExpression methodCall = Expression.Call(param, methodInfo);
        Expression<Func<IEntity, object>> lambda = Expression.Lambda<Func<IEntity, object>>(
            methodCall,
            param
        );

        _keyLookups[typeof(TEntity)] = lambda.Compile();
    }
}
