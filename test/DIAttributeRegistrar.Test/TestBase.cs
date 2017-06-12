using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DIAttributeRegistrar.Test
{
    public abstract class TestBase
    {
        protected IServiceCollection ServiceCollection { get; set; }

        protected Assembly CurrentAssembly { get; set; }

        protected Assembly[] CurrentAssemblies { get; set; }

        public TestBase()
        {
            this.ServiceCollection = new ServiceCollection();
            this.CurrentAssembly = this.GetType().GetTypeInfo().Assembly;
            this.CurrentAssemblies = new Assembly[] { CurrentAssembly };
        }
    }
}
