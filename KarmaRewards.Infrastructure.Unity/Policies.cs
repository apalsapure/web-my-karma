using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Infrastructure.Unity
{
    internal class EmptyArray
    {
        public static readonly object[] Instance = new object[0];
    }

    public class CustomContainerExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Policies.SetDefault<IConstructorSelectorPolicy>(
                new InjectionConstructorSelectorPolicy());
            Context.Policies.SetDefault<IPropertySelectorPolicy>(
                new InjectionPropertySelectorPolicy());
            Context.Policies.SetDefault<IMethodSelectorPolicy>(
                new InjectionMethodSelectorPolicy());
        }
    }

    public class InjectionConstructorSelectorPolicy : ConstructorSelectorPolicyBase<InjectionConstructorAttribute>
    {
        protected override IDependencyResolverPolicy CreateResolver(ParameterInfo param)
        {
            // Resolve all DependencyAttributes on this parameter, if any
            var attribute = (param.GetCustomAttributes(false) ?? EmptyArray.Instance)
                .Where<object>(x => x is DependencyAttribute)
                .Cast<DependencyAttribute>()
                .SingleOrDefault<DependencyAttribute>();

            if (attribute != null)
                return new NamedTypeDependencyResolverPolicy(param.ParameterType, attribute.Name);
            else
                return new NamedTypeDependencyResolverPolicy(param.ParameterType, null);
        }
    }


    public class InjectionPropertySelectorPolicy : PropertySelectorBase<DependencyAttribute>
    {
        protected override IDependencyResolverPolicy CreateResolver(PropertyInfo property)
        {
            var attribute = (property.GetCustomAttributes(typeof(DependencyAttribute), false) ?? EmptyArray.Instance)
                .Where<object>(o => o is DependencyAttribute)
                .Cast<DependencyAttribute>()
                .SingleOrDefault<DependencyAttribute>();
            if (attribute == null)
                return new NamedTypeDependencyResolverPolicy(property.PropertyType, null);
            else
                return new NamedTypeDependencyResolverPolicy(property.PropertyType, attribute.Name);
        }
    }

    public class InjectionMethodSelectorPolicy : MethodSelectorPolicyBase<InjectionMethodAttribute>
    {

        protected override IDependencyResolverPolicy CreateResolver(ParameterInfo parameter)
        {
            // Resolve all DependencyAttributes on this parameter, if any
            var attribute = (parameter.GetCustomAttributes(false) ?? EmptyArray.Instance)
                .Where<object>(x => x is DependencyAttribute)
                .Cast<DependencyAttribute>()
                .SingleOrDefault<DependencyAttribute>();

            if (attribute != null)
                return new NamedTypeDependencyResolverPolicy(parameter.ParameterType, attribute.Name);
            else return new NamedTypeDependencyResolverPolicy(parameter.ParameterType, null);
        }
    }
}
