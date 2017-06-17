using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DIAttributeRegistrar
{
    internal class AttributeRegistrar
    {
        internal AttributeRegistrar(Func<IEnumerable<Assembly>> assemblySearchMethod = null)
        {
            this.assemblySearchMethod = assemblySearchMethod ?? GetAllAssemblies;
        }

        private Func<IEnumerable<Assembly>> assemblySearchMethod;

        internal void RegisterType(IServiceCollection services, ServiceLifetime serviceLifetime, Type toRegister, Type asType = null)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(asType ?? toRegister, toRegister);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped(asType ?? toRegister, toRegister);
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient(asType ?? toRegister, toRegister);
                    break;
                default:
                    throw new NotSupportedException($"{Enum.GetName(typeof(ServiceLifetime), serviceLifetime)} is not supported.");
            }
        }

        internal void RegisterTypes(IServiceCollection services, IEnumerable<Type> types, string[] requiredTags)
        {
            requiredTags = requiredTags ?? new string[] { };

            if (types != null)
            {
                foreach (var type in types)
                {
                    var asTypeGroups = type
                        .GetTypeInfo()
                        .GetCustomAttributes<RegisterAttribute>(false)
                        .Where(ra => IsRegisterAttributeTagValid(ra.Tags, requiredTags))
                        .GroupBy(t => t.AsType);

                    foreach (var asTypeGroup in asTypeGroups)
                    {
                        var selectedAttribute = asTypeGroup.OrderByDescending(t => t.ServiceLifetime).FirstOrDefault();
                        RegisterType(services, selectedAttribute.ServiceLifetime, type, selectedAttribute.AsType);
                    }
                }
            }
        }

        internal void RegisterTypes(IServiceCollection services, IEnumerable<Assembly> assemblies, string[] requiredTags)
        {
            if (assemblies != null)
            {
                IEnumerable<Type> types = assemblies
                    .SelectMany(t => t.ExportedTypes)
                    .Where(t => t.GetTypeInfo().IsDefined(typeof(RegisterAttribute)));

                this.RegisterTypes(services, types, requiredTags);
            }
        }

        internal void RegisterTypes(IServiceCollection services, string[] requiredTags)
        {
            IEnumerable<Assembly> assemblies = assemblySearchMethod();
            if (assemblies != null)
            {
                this.RegisterTypes(services, assemblies, requiredTags);
            }
        }

        private bool IsRegisterAttributeTagValid(string[] attributeTags, string[] requiredTags)
        {
            if (requiredTags.Length == 0)
            {
                return attributeTags.Length == 0;
            }
            else
            {
                return attributeTags.Length == 0 || requiredTags.All(rt => attributeTags.Any(at => at.Equals(rt, StringComparison.OrdinalIgnoreCase)));
            }
        }

        private IEnumerable<Assembly> GetAllAssemblies()
        {
            var currentAssemblyFullName = this.GetType().GetTypeInfo().Assembly.GetName().FullName;
            return DependencyContext
                .Default
                .RuntimeLibraries
                .SelectMany(l => l.GetDefaultAssemblyNames(DependencyContext.Default))
                .Select(Assembly.Load)
                .Where(t =>
                    t.GetReferencedAssemblies()
                        .Any(ra => 0 == String.Compare(ra.FullName, currentAssemblyFullName, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
