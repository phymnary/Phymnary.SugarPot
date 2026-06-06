using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public class AspInvalidOperationException(string message, Exception? innerException = null)
    : Exception(message, innerException),
        IApplicationException
{
    public HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;

    public string? ErrorCode { get; private set; }

    public AspInvalidOperationException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
