using System.Net;

namespace Phymnary.SugarPot.AspNetCore;

public interface IAspException
{
    HttpStatusCode StatusCode { get; }

    string? ErrorCode => null;
}
