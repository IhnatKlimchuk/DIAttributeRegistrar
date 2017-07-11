using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DIAttributeRegistrar.AssemblyDiscovery
{
    internal class DefaultAssemblyDiscoveryProvider : IAssemblyDiscoveryProvider
    {
        private static HashSet<string> ReferenceAssemblies { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "DIAttributeRegistrar"
        };

        public IEnumerable<Assembly> GetCandidateAssemblies()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            DependencyContext context = DependencyContext.Load(entryAssembly);

            CandidateResolver candidateResolver = new CandidateResolver(context.RuntimeLibraries, ReferenceAssemblies);

            return candidateResolver
                .GetCandidates()
                .SelectMany(library => library.GetDefaultAssemblyNames(context))
                .Select(Assembly.Load);
        }
    }
}
