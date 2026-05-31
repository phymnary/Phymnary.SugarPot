using System.Net;

namespace AspNetCore.Boilerplate;

public class AspInvalidOperationException(string message)
    : InvalidOperationException(message),
        IAspException
{
    public HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;

    public string? ErrorCode { get; private set; }

    public AspInvalidOperationException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
