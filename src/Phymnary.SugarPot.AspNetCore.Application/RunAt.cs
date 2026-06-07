namespace Phymnary.SugarPot.AspNetCore;

internal class RunAt : IRunAt
{
    public DateTimeOffset Value { get; } = DateTimeOffset.UtcNow;
}
