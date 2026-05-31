namespace AspNetCore.Boilerplate.Api;

public class RequestedAt
{
    public DateTimeOffset Value { get; init; } = DateTimeOffset.UtcNow;
}
