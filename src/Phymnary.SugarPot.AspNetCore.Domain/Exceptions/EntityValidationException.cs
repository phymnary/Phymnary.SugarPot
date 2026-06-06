using System.Net;
using Phymnary.SugarPot.AspNetCore.Entities;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public class EntityValidationException(string message, Exception? innerException = null)
    : Exception(message, innerException),
        IDomainException
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
