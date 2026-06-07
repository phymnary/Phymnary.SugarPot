using Microsoft.Extensions.DependencyInjection;

namespace Phymnary.SugarPot.AspNetCore;

public class ScopeContext
{
    public Guid? CurrentUserId { get; init; }

    public Guid? CurrentTenantId { get; init; }

    public CancellationToken? RequestAborted { get; init; }
}

public interface IScopeBuilder
{
    AsyncServiceScope Initialize(ScopeContext context);
}
