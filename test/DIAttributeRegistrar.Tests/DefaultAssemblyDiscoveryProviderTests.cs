using Microsoft.Extensions.DependencyModel;
using System.Linq;

namespace DIAttributeRegistrar.Tests
{
    public class DefaultAssemblyDiscoveryProviderTests : TestBase
    {
        public DefaultAssemblyDiscoveryProviderTests() : base()
        {

        }
        
        private static RuntimeLibrary GetLibrary(string name, params string[] dependencyNames)
        {
            var dependencies = dependencyNames?.Select(d => new Microsoft.Extensions.DependencyModel.Dependency(d, "42.0.0")) ?? new Microsoft.Extensions.DependencyModel.Dependency[0];

            return new RuntimeLibrary(
                "package",
                name,
                "23.0.0",
                "hash",
                new RuntimeAssetGroup[0],
                new RuntimeAssetGroup[0],
                new ResourceAssembly[0],
                dependencies: dependencies.ToArray(),
                serviceable: true);
        }
    }
}
