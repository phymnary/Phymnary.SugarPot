using System.Net;

namespace Phymnary.SugarPot.AspNetCore;

public class EntityNotFoundException(string message) : Exception(message), IAspException
{
    public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

    public string? ErrorCode { get; private set; }

    public EntityNotFoundException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
