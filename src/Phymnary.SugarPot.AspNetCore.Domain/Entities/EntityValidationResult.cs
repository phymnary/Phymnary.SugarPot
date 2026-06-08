namespace Phymnary.SugarPot.AspNetCore.Entities;

public class EntityValidationResult
{
    public required bool IsValid { get; init; }

    public required EntityValidationFailureDetail[] Errors { get; init; }

    public static EntityValidationResult Valid =
   new EntityValidationResult
   {
       IsValid = true,
       Errors = []
   };
    
}
