using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public class AspBadRequestException(string message) : Exception(message), IDomainException
{
    public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    public string? ErrorCode { get; private set; }

    public AspBadRequestException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
