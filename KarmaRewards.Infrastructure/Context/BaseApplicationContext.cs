using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KarmaRewards.Infrastructure.Context
{
    [Serializable]
    public abstract class BaseApplicationContext : IApplicationContext
    {
        protected BaseApplicationContext()
        {
            this._items = new ConcurrentDictionary<string, ObjectContainer?>(StringComparer.OrdinalIgnoreCase);
            // Propagate the correlation id to all new instances.
            var context = ApplicationContextScope.Current;
            this.CorrelationId = context == null ? Guid.NewGuid().ToString() : context.CorrelationId;
        }

        private ConcurrentDictionary<string, ObjectContainer?> _items;

        public T Get<T>(string name, T defaultValue = default(T))
        {
            ObjectContainer? value;
            if (_items.TryGetValue(name, out value) == true)
            {
                if (value != null)
                    return value.Value.GetState<T>();
            }
            return defaultValue;
        }

        public string CorrelationId { get; private set; }

        bool _isDisposed = false;
        public void Dispose()
        {
            if (_isDisposed == true) return;
            Dispose(true);
            GC.SuppressFinalize(this);
            _isDisposed = true;
        }

        protected void Dispose(bool isDisposing)
        {
            if (isDisposing == true)
            {
                foreach (var container in _items.Values)
                {
                    try
                    {
                        if (container != null)
                            container.Value.Teardown();
                    }
                    catch { }
                }
            }
        }

        public void Set(string name, object value)
        {
            Set(name, value, null);
        }

        public void SetAsDisposable(string name, IDisposable value)
        {
            Set(name, value, new ContextControllerLifetimeManager());
        }

        public void Set(string name, object value, ILifetimeManager lifetimeManager = null)
        {
            this._items[name] = new ObjectContainer(value, lifetimeManager);
        }

        public static IDisposable CreateScope(IApplicationContext appContext)
        {
            return new ApplicationContextScope(appContext);
        }
    }
}
