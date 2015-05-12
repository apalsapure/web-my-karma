using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KarmaRewards.Infrastructure.Mvc
{
    public interface IActionFilterFactory
    {
        IEnumerable<IStatelessActionFilter> CreateFilters();
    }
}
