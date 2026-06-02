using Phymnary.SugarPot.AspNetCore.Entities;
using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public class EntityValidationException(string message) : Exception(message), IDomainException
{
    public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    public string? ErrorCode { get; private set; } =
        DomainErrorCodeRegistry.DefaultEntityValidationErrorCode;

    public required EntityValidationFailureDetail[] Failures { get; init; }

    public EntityValidationException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
