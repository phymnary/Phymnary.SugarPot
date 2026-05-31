using System.Net;

namespace AspNetCore.Boilerplate;

public class InternalServiceUnavailableException(string message) : Exception(message), IAspException
{
    public HttpStatusCode StatusCode => HttpStatusCode.ServiceUnavailable;

    public string? ErrorCode { get; private set; }

    public InternalServiceUnavailableException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
