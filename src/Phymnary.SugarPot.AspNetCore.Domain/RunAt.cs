namespace Phymnary.SugarPot.AspNetCore;

public interface IRunAt
{
    DateTimeOffset Value { get; }
}

public class RunAt : IRunAt
{
    public DateTimeOffset Value { get; } = DateTimeOffset.UtcNow;
}
