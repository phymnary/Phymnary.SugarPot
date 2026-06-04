using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public class AspUnauthorizedException(string message) : Exception(message), IApplicationException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

    public string? ErrorCode { get; private set; }

    public AspUnauthorizedException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
