namespace Phymnary.SugarPot.AspNetCore.Entities;

public class EntityValidationResult
{
    public required bool IsValid { get; init; }

    public required EntityValidationFailureDetail[] Errors { get; init; }
}
