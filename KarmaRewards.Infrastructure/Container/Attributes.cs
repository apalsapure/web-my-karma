using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class DependencyAttribute : Attribute
    {
        public DependencyAttribute() : this(null) { }

        public DependencyAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Constructor)]
    public sealed class InjectionConstructorAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InjectionMethodAttribute : Attribute
    {
    }
}
