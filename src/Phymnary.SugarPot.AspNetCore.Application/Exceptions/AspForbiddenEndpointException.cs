using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public class AspForbiddenEndpointException(string message, Exception? innerException = null)
    : Exception(message, innerException),
        IApplicationException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Forbidden;

    public string? ErrorCode { get; private set; }

    public AspForbiddenEndpointException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
