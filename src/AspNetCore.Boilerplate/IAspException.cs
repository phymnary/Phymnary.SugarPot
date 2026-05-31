using System.Net;

namespace AspNetCore.Boilerplate;

public interface IAspException
{
    HttpStatusCode StatusCode { get; }

    string? ErrorCode => null;
}
