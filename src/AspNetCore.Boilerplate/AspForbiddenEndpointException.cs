using System.Net;

namespace AspNetCore.Boilerplate;

public class AspForbiddenEndpointException(string message) : Exception(message), IAspException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Forbidden;

    public string? ErrorCode { get; private set; }

    public AspForbiddenEndpointException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
