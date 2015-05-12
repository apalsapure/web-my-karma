using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KarmaRewards.Infrastructure.Context
{
    public class ContextControllerLifetimeManager : ILifetimeManager
    {
        public void Dispose(object state)
        {
            var disposable = state as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }
    }
}
