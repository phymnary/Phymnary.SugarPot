using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public class EntityNotFoundException(string message) : Exception(message), IDomainException
{
    public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

    public string? ErrorCode { get; private set; } =
        DomainErrorCodeRegistry.DefaultEntityNotFoundErrorCode;

    public EntityNotFoundException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
