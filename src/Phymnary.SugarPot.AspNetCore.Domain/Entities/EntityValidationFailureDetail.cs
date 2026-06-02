namespace Phymnary.SugarPot.AspNetCore.Entities;

public class EntityValidationFailureDetail
{
    public required string Property { get; init; }

    public required string Message { get; init; }

    public string? Code { get; init; }
}
