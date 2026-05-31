namespace Phymnary.SugarPot.AspNetCore.Api;

public class HttpContextProvider
{
    public Guid? CurrentUserId { get; init; }

    public Guid? CurrentTenantId { get; init; }

    public CancellationToken? RequestAborted { get; init; }
}
