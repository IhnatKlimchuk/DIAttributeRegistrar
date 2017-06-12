using Microsoft.Extensions.DependencyModel;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIAttributeRegistrarExtentions
    {
        /// <summary>
        ///     <para>
        ///         Provides ability initialize all attribute marked dependencies.
        ///     </para>
        /// </summary>
        /// <param name="serviceCollection">Service collection to add dependencies.</param>
        /// <param name="assembliesToSearch">Assemblies to search <see cref="RegisterAttribute"/> marked classes.</param>
        /// <param name="tags">Tags to filter attributes. See documentation for more details.</param>
        public static IServiceCollection AddAttributeRegistration(this IServiceCollection serviceCollection, Assembly[] assembliesToSearch, params string[] tags)
        {
            ConfigureManagersFromAttributes(serviceCollection, assembliesToSearch, tags);
            return serviceCollection;
        }

        /// <summary>
        ///     <para>
        ///         Provides ability initialize all attribute marked dependencies.
        ///     </para>
        /// </summary>
        /// <param name="serviceCollection">Service collection to add dependencies.</param>
        /// <param name="tags">Tags to filter attributes. See documentation for more details.</param>
        public static IServiceCollection AddAttributeRegistration(this IServiceCollection serviceCollection, params string[] tags)
        {
            var assemblies = GetAllAssemblies();
            return AddAttributeRegistration(serviceCollection, assemblies, tags);
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

        private static void ConfigureManagersFromAttributes(IServiceCollection services, Assembly[] assemblies, string[] requiredTags)
        {
            requiredTags = requiredTags ?? new string[] { };

            var typesToRegister = assemblies
                .SelectMany(t => t.ExportedTypes)
                .Where(t => t.GetTypeInfo().IsDefined(typeof(RegisterAttribute)))
                .Select(type =>
                    new
                    {
                        Type = type,
                        AttributeValues = type.GetTypeInfo().GetCustomAttributes<RegisterAttribute>(false).Where(ra => FilterRegisterAttribute(ra.Tags, requiredTags))
                    })
                .Where(t => t.AttributeValues != null);

            foreach (var typeToRegister in typesToRegister)
            {
                foreach (var asType in typeToRegister.AttributeValues.GroupBy(t => t.AsType))
                {
                    var selectedAttribute = asType.OrderByDescending(t => t.ServiceLifetime).FirstOrDefault();
                    Register(services, selectedAttribute.ServiceLifetime, typeToRegister.Type, selectedAttribute.AsType);
                }
            }
        }

        private static bool FilterRegisterAttribute(string[] attributeTags, string[] requiredTags)
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
    }
}
