using System.Net;

namespace AspNetCore.Boilerplate;

public class AspUnauthorizedException(string message) : Exception(message), IAspException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

    public string? ErrorCode { get; private set; }

    public AspUnauthorizedException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
