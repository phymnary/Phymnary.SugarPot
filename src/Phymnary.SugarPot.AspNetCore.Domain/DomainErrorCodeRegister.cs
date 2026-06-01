using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phymnary.SugarPot.AspNetCore.Domain;

public static class DomainErrorCodeRegister
{
    public static string? DefaultEntityNotFoundErrorCode { get; set; }

    public static string? DefaultEntityValidationErrorCode { get; set; }
}
