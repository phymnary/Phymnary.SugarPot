using System.Net;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public interface IBusinessException
{
    HttpStatusCode StatusCode { get; }

    string? ErrorCode => null;
}
