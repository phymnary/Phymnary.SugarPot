using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public class AspUnauthorizedException(string message, Exception? innerException = null)
    : Exception(message, innerException),
        IApplicationException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

    public string? ErrorCode { get; private set; }

    public AspUnauthorizedException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
