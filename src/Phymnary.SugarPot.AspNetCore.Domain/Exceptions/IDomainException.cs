using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public interface IDomainException
{
    HttpStatusCode StatusCode { get; }

    string? ErrorCode => null;
}
