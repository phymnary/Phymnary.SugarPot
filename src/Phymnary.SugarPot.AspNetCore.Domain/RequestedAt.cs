using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phymnary.SugarPot.AspNetCore;

public interface IRequestedAt
{
    DateTimeOffset Value { get; }
}

public class RequestedAt : IRequestedAt
{
    public DateTimeOffset Value { get; } = DateTimeOffset.UtcNow;
}
