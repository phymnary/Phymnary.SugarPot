using System.Net;
using AspNetCore.Boilerplate.Api.ExceptionHandler;

namespace AspNetCore.Boilerplate;

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
