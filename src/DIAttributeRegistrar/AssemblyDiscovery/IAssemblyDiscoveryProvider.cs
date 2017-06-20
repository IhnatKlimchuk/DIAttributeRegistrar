using System.Collections.Generic;
using System.Reflection;

namespace DIAttributeRegistrar.AssemblyDiscovery
{
    internal interface IAssemblyDiscoveryProvider
    {
        IEnumerable<Assembly> GetCandidateAssemblies();
    }
}
