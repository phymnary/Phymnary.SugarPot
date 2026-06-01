using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Domain.Exceptions;

public interface IDomainException
{
    HttpStatusCode StatusCode { get; }

    string? ErrorCode => null;
}
