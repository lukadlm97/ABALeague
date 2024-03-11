using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Contracts
{
    public interface ICacheInvalidatorMediatrQuery<in TRequest>
    {
        bool BypassCache { get; }
        string CacheKey { get; }
    }
}
