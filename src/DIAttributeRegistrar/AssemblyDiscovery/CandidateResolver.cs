using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;

namespace DIAttributeRegistrar.AssemblyDiscovery
{
    internal class CandidateResolver
    {
        private readonly IDictionary<string, Dependency> runtimeDependencies;

        public CandidateResolver(IReadOnlyList<RuntimeLibrary> runtimeDependencies, ISet<string> referenceAssemblies)
        {
            var dependenciesWithNoDuplicates = new Dictionary<string, Dependency>(StringComparer.OrdinalIgnoreCase);

            foreach (var dependency in runtimeDependencies)
            {
                if (dependenciesWithNoDuplicates.ContainsKey(dependency.Name))
                {
                    throw new InvalidOperationException($"Duplicate runtime dependency found.");
                }
                dependenciesWithNoDuplicates.Add(dependency.Name, CreateDependency(dependency, referenceAssemblies));
            }

            this.runtimeDependencies = dependenciesWithNoDuplicates;
        }

        public IEnumerable<RuntimeLibrary> GetCandidates()
        {
            foreach (var dependency in runtimeDependencies)
            {
                if (ComputeClassification(dependency.Key) == DependencyClassification.ReferencesAttributeRegistrar)
                {
                    yield return dependency.Value.Library;
                }
            }
        }

        private Dependency CreateDependency(RuntimeLibrary library, ISet<string> referenceAssemblies)
        {
            var classification = DependencyClassification.Unknown;
            if (referenceAssemblies.Contains(library.Name))
            {
                classification = DependencyClassification.AttributeRegistrarReference;
            }

            return new Dependency(library, classification);
        }

        private DependencyClassification ComputeClassification(string dependency)
        {
            if (!runtimeDependencies.ContainsKey(dependency))
            {
                return DependencyClassification.DoesNotReferenceAttributeRegistrar;
            }

            var candidateEntry = runtimeDependencies[dependency];
            if (candidateEntry.Classification != DependencyClassification.Unknown)
            {
                return candidateEntry.Classification;
            }
            else
            {
                var classification = DependencyClassification.DoesNotReferenceAttributeRegistrar;
                foreach (var candidateDependency in candidateEntry.Library.Dependencies)
                {
                    var dependencyClassification = ComputeClassification(candidateDependency.Name);
                    if (dependencyClassification == DependencyClassification.ReferencesAttributeRegistrar ||
                        dependencyClassification == DependencyClassification.AttributeRegistrarReference)
                    {
                        classification = DependencyClassification.ReferencesAttributeRegistrar;
                        break;
                    }
                }

                candidateEntry.Classification = classification;

                return classification;
            }
        }
    }
}
