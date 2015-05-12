using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KarmaRewards.Infrastructure.Context
{
    internal struct ObjectContainer
    {
        public ObjectContainer(object state, ILifetimeManager lifetimeManager)
            : this()
        {
            this.State = state;
            this.LifetimeManager = lifetimeManager;
        }
        public object State { get; private set; }

        public ILifetimeManager LifetimeManager { get; private set; }

        public void Teardown()
        {
            if (this.LifetimeManager != null)
                this.LifetimeManager.Dispose(this.State);
        }

        public T GetState<T>(T defaultValue = default(T))
        {
            if (this.State == null || this.State is T == false)
                return defaultValue;
            else return (T)this.State;
        }
    }
}
