using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public class DomainNotImplementedException(string message) : Exception(message), IDomainException
{
    public HttpStatusCode StatusCode => HttpStatusCode.UnprocessableContent;
}
