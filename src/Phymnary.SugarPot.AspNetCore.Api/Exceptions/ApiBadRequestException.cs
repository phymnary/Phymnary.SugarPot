using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public class ApiBadRequestException(string message) : Exception(message), ISolutionException
{
    public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    public string? ErrorCode { get; private set; }

    public ApiBadRequestException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
