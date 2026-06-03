namespace Phymnary.SugarPot.AspNetCore;

public interface IRequestedAt
{
    DateTimeOffset Value { get; }
}

public class RequestedAt : IRequestedAt
{
    public DateTimeOffset Value { get; } = DateTimeOffset.UtcNow;
}
