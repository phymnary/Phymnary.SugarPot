namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public static class DomainErrorCodeRegistry
{
    public static string? DefaultEntityNotFoundErrorCode { get; set; }

    public static string? DefaultEntityValidationErrorCode { get; set; }

    public static string? DefaultTenantMissingInContextException { get; set; }

    public static string? DefaultEntityPersistenceException { get; set; }
}
