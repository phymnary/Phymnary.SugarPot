using AspNetCore.Boilerplate.Domain;
using FluentValidation;

namespace Phymnary.SugarPot.AspNetCore.Extensions;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WhenCreating<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> ruleBuilder
    )
        where T : IAuditable
    {
        return ruleBuilder.When(entity => entity.CreatedAt == default);
    }
}
