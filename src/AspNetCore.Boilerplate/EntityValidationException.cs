using System.Net;
using Phymnary.SugarPot.AspNetCore.Api.ExceptionHandler;

namespace Phymnary.SugarPot.AspNetCore;

public class EntityValidationException(string message) : Exception(message), IAspException
{
    public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    public string? ErrorCode { get; private set; }

    public required AspValidationErrorDetail[] Failures { get; init; }

    public EntityValidationException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
