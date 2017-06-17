using DIAttributeRegistrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIAttributeRegistrarExtentions
    {
        /// <summary>
        ///     <para>
        ///         Provides ability to initialize all attribute marked dependencies.
        ///     </para>
        /// </summary>
        /// <param name="serviceCollection">Service collection to add dependencies.</param>
        /// <param name="assembliesToSearch">Assemblies to search <see cref="RegisterAttribute"/> marked classes.</param>
        /// <param name="tags">Tags to filter attributes. See documentation for more details.</param>
        [Obsolete]
        public static IServiceCollection AddAttributeRegistration(this IServiceCollection serviceCollection, Assembly[] assembliesToSearch, params string[] tags)
        {
            return AddAttributeRegistration(serviceCollection, assembliesToSearch.AsEnumerable(), tags);
        }

        /// <summary>
        ///     <para>
        ///         Provides ability to initialize all attribute marked dependencies.
        ///     </para>
        /// </summary>
        /// <param name="serviceCollection">Service collection to add dependencies.</param>
        /// <param name="assembliesToSearch">Assemblies to search <see cref="RegisterAttribute"/> marked classes.</param>
        /// <param name="tags">Tags to filter attributes. See documentation for more details.</param>
        public static IServiceCollection AddAttributeRegistration(this IServiceCollection serviceCollection, IEnumerable<Assembly> assembliesToSearch, params string[] tags)
        {
            AttributeRegistrar attributeRegistrar = new AttributeRegistrar();
            attributeRegistrar.RegisterTypes(serviceCollection, assembliesToSearch, tags);
            return serviceCollection;
        }

        /// <summary>
        ///     <para>
        ///         Provides ability to initialize all attribute marked dependencies.
        ///     </para>
        /// </summary>
        /// <param name="serviceCollection">Service collection to add dependencies.</param>
        /// <param name="assembliesToSearch">Assemblies to search <see cref="RegisterAttribute"/> marked classes.</param>
        /// <param name="tags">Tags to filter attributes. See documentation for more details.</param>
        public static IServiceCollection AddAttributeRegistration(this IServiceCollection serviceCollection, IEnumerable<Type> typesToSearch, params string[] tags)
        {
            AttributeRegistrar attributeRegistrar = new AttributeRegistrar();
            attributeRegistrar.RegisterTypes(serviceCollection, typesToSearch, tags);
            return serviceCollection;
        }

        /// <summary>
        ///     <para>
        ///         Provides ability to initialize all attribute marked dependencies.
        ///     </para>
        /// </summary>
        /// <param name="serviceCollection">Service collection to add dependencies.</param>
        /// <param name="tags">Tags to filter attributes. See documentation for more details.</param>
        public static IServiceCollection AddAttributeRegistration(this IServiceCollection serviceCollection, params string[] tags)
        {
            AttributeRegistrar attributeRegistrar = new AttributeRegistrar();
            attributeRegistrar.RegisterTypes(serviceCollection, tags);
            return serviceCollection;
        }
    }
}
