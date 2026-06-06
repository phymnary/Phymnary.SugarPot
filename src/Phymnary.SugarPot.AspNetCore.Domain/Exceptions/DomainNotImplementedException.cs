using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public class DomainNotImplementedException(string message, Exception? innerException = null)
    : Exception(message, innerException),
        IDomainException
{
    public HttpStatusCode StatusCode => HttpStatusCode.UnprocessableContent;
}
