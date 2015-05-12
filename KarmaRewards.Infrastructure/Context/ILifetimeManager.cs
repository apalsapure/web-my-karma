using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KarmaRewards.Infrastructure.Context
{
    public interface ILifetimeManager
    {
        void Dispose(object state);
    }
}
