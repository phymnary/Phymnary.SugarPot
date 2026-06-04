using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public class ApiInvalidOperationException(string message)
    : InvalidOperationException(message),
        ISolutionException
{
    public HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;

    public string? ErrorCode { get; private set; }

    public ApiInvalidOperationException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
