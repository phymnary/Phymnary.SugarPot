using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public class ApiForbiddenEndpointException(string message) : Exception(message), ISolutionException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Forbidden;

    public string? ErrorCode { get; private set; }

    public ApiForbiddenEndpointException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
