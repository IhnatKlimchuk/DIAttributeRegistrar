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
                if (ComputeClassification(dependency.Key) == DependencyClassification.ReferencesAttributeRegistar)
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
                classification = DependencyClassification.AttributeRegistarReference;
            }

            return new Dependency(library, classification);
        }

        private DependencyClassification ComputeClassification(string dependency)
        {
            if (!runtimeDependencies.ContainsKey(dependency))
            {
                return DependencyClassification.DoesNotReferenceAttributeRegistar;
            }

            var candidateEntry = runtimeDependencies[dependency];
            if (candidateEntry.Classification != DependencyClassification.Unknown)
            {
                return candidateEntry.Classification;
            }
            else
            {
                var classification = DependencyClassification.DoesNotReferenceAttributeRegistar;
                foreach (var candidateDependency in candidateEntry.Library.Dependencies)
                {
                    var dependencyClassification = ComputeClassification(candidateDependency.Name);
                    if (dependencyClassification == DependencyClassification.ReferencesAttributeRegistar ||
                        dependencyClassification == DependencyClassification.AttributeRegistarReference)
                    {
                        classification = DependencyClassification.ReferencesAttributeRegistar;
                        break;
                    }
                }

                candidateEntry.Classification = classification;

                return classification;
            }
        }
    }
}
