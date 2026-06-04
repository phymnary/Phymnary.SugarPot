using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public class ApiUnauthorizedException(string message) : Exception(message), ISolutionException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

    public string? ErrorCode { get; private set; }

    public ApiUnauthorizedException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
