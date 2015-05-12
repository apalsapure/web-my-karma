using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace KarmaRewards.Infrastructure.Context
{
    [Serializable]
    public abstract class BaseApplicationContextScope<T> : IDisposable
        where T : class
    {

        protected BaseApplicationContextScope(T context)
        {
            Push(context);
        }

        public static readonly string Name = Guid.NewGuid().ToString("N");

        public static ImmutableStack<T> Contexts
        {
            get
            {
                var wrapper = CallContext.LogicalGetData(Name) as Wrapper<T>;
                return wrapper == null ? ImmutableStack.Create<T>() : wrapper.Value;
            }
            set
            {
                CallContext.LogicalSetData(Name, new Wrapper<T>() { Value = value });
            }
        }

        public static T Current
        {
            get
            {
                var contexts = Contexts;
                if (Contexts.Count() == 0)
                    return null;
                else
                    return contexts.Peek();
            }
        }

        public void Push(T context)
        {
            Contexts = Contexts.Push(context);
        }

        public void Pop()
        {
            T context;
            if (Contexts.IsEmpty == true)
                Contexts = null;
            else
            {
                Contexts = Contexts.Pop(out context);
                DisposeContext(context as IDisposable);
            }
        }

        private void DisposeContext(IDisposable context)
        {
            if (context != null)
                context.Dispose();
        }



        bool _isDisposed = false;
        public void Dispose()
        {
            if (_isDisposed == false)
            {
                _isDisposed = true;
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing == true)
            {
                Pop();
            }
        }

        /* 
         * Wrapper class added to ensure that the scope works across cross app domain calls.
         * These are applicable in scenarios like unit tests which are run in a separate app domain e.g., MSTEST
         * Ref: http://blog.stephencleary.com/2013/04/implicit-async-context-asynclocal.html
         * - Nikhil
         */
        private sealed class Wrapper<T> : MarshalByRefObject
        {
            public ImmutableStack<T> Value { get; set; }
        }
    }
}
