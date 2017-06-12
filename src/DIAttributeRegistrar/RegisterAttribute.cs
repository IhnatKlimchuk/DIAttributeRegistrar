using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     <para>
    ///         Provides ability to setup dependecy injection for marked class.
    ///     </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RegisterAttribute : Attribute
    {
        public ServiceLifetime ServiceLifetime { get; private set; }

        public Type AsType { get; private set; }

        public string[] Tags { get; private set; }

        /// <summary>
        ///     <para>
        ///         Provides ability to setup dependecy injection for marked class.
        ///     </para>
        /// </summary>
        /// <param name="asType">Type to register as. The default value is the same as type of market class.</param>
        /// <param name="serviceLifetime">Lifetime of the registered service. The default value is <see cref="ServiceLifetime.Scoped"/>.</param>
        /// <param name="tags">Tags to associate dependecy with.</param>
        public RegisterAttribute(Type asType = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, params string[] tags)
        {
            this.ServiceLifetime = serviceLifetime;
            this.AsType = asType;
            this.Tags = tags == null ? new string[] { } : tags.Where(t => t != null).ToArray();
        }

        /// <summary>
        ///     <para>
        ///         Provides ability to setup dependecy injection for marked class.
        ///     </para>
        /// </summary>
        /// <param name="serviceLifetime">Lifetime of the registered service. The default value is <see cref="ServiceLifetime.Scoped"/>.</param>
        /// <param name="tags">Tags to associate dependecy with.</param>
        public RegisterAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, params string[] tags) : this(null, serviceLifetime, tags)
        {

        }

        /// <summary>
        ///     <para>
        ///         Provides ability to setup dependecy injection for marked class.
        ///     </para>
        /// </summary>
        /// <param name="tags">Tags to associate dependecy with.</param>
        public RegisterAttribute(params string[] tags) : this(null, ServiceLifetime.Scoped, tags)
        {

        }


        /// <summary>
        ///     <para>
        ///         Provides ability to setup dependecy injection for marked class.
        ///     </para>
        /// </summary>
        public RegisterAttribute() : this(null)
        {

        }
    }
}
