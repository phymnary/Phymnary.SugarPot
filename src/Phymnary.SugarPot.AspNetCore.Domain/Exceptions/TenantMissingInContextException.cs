using Phymnary.SugarPot.AspNetCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public class TenantMissingInContextException(string message) : Exception(message), IDomainException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Forbidden;

    public string? ErrorCode { get; private set; } =
           DomainErrorCodeRegistry.DefaultTenantMissingInContextException;

    public TenantMissingInContextException WithErrorCode(string code)
    {
        ErrorCode = code;
        return this;
    }
}
