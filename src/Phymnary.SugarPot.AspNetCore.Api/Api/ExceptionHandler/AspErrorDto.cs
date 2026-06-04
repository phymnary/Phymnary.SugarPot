using Phymnary.SugarPot.AspNetCore.Entities;

namespace Phymnary.SugarPot.AspNetCore.Api.ExceptionHandler;

public class AspErrorDto
{
    public required AspErrorMessage Error { get; init; }
}

public class AspErrorMessage
{
    public required string Message { get; init; }

    public string? Code { get; init; }

    public string? Detail { get; init; }

    public IEnumerable<EntityValidationFailureDetail>? InvalidParameters { get; init; }
}
