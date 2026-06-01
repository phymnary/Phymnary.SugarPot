namespace Phymnary.SugarPot.AspNetCore.Api;

public class RequestedAt
{
    public DateTimeOffset Value { get; init; } = DateTimeOffset.UtcNow;
}
