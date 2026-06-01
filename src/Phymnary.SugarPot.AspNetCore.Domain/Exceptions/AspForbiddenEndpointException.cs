using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Domain.Exceptions;

public class AspForbiddenEndpointException(string message) : Exception(message), IDomainException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Forbidden;

    public string? ErrorCode { get; private set; }

    public AspForbiddenEndpointException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
