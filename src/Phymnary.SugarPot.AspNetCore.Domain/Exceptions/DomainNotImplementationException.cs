using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Phymnary.SugarPot.AspNetCore.Exceptions;

public class DomainNotImplementationException(string message) : Exception(message), IDomainException
{
    public HttpStatusCode StatusCode => HttpStatusCode.UnprocessableContent;
}
