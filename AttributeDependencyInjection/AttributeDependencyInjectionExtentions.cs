using Microsoft.Extensions.DependencyModel;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AttributeDependencyInjectionExtentions
    {
        public static IServiceCollection AddAttributeRegistration(this IServiceCollection serviceCollection, Assembly[] assembliesToSearch, string groupName = null)
        {
            ConfigureManagersFromAttributes(serviceCollection, assembliesToSearch, groupName);
            return serviceCollection;
        }

        public static IServiceCollection AddAttributeRegistration(this IServiceCollection serviceCollection, Assembly[] assembliesToSearch, params string[] groupNames)
        {
            foreach (var group in groupNames)
            {
                ConfigureManagersFromAttributes(serviceCollection, assembliesToSearch, group);
            }
            return serviceCollection;
        }

        public static IServiceCollection AddAttributeRegistration(this IServiceCollection serviceCollection, string groupName = null)
        {
            return AddAttributeRegistration(serviceCollection, GetAllAssemblies(), groupName);
        }

        public static IServiceCollection AddAttributeRegistration(this IServiceCollection serviceCollection, params string[] groupNames)
        {
            return AddAttributeRegistration(serviceCollection, GetAllAssemblies(), groupNames);
        }

        private static Assembly[] GetAllAssemblies()
        {
            return DependencyContext.Default.RuntimeLibraries.SelectMany(l => l.GetDefaultAssemblyNames(DependencyContext.Default)).Select(Assembly.Load).ToArray();
        }

        private static void Register(IServiceCollection services, ServiceLifetime serviceLifetime, Type toRegister, Type asType = null)
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

        private static void ConfigureManagersFromAttributes(IServiceCollection services, Assembly[] assemblies, string groupName = null)
        {
            var classesToRegister = assemblies
                .SelectMany(t => t.GetTypes())
                .Where(t => t.GetTypeInfo().IsDefined(typeof(RegisterAttribute)))
                .Select(t =>
                    new
                    {
                        type = t,
                        attributeValue = t.GetTypeInfo().GetCustomAttributes<RegisterAttribute>(false).FirstOrDefault(at => groupName == null ? at.GroupName == null : at.GroupName == groupName)
                    })
                .Where(t => t.attributeValue != null);

            foreach (var item in classesToRegister)
            {
                Register(services, item.attributeValue.ServiceLifetime, item.type, item.attributeValue.AsType);
            }
        }
    }
}
