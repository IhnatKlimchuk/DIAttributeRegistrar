using Microsoft.Extensions.DependencyModel;

namespace DIAttributeRegistrar.AssemblyDiscovery
{
    internal class Dependency
    {
        public Dependency(RuntimeLibrary library, DependencyClassification classification)
        {
            Library = library;
            Classification = classification;
        }

        public RuntimeLibrary Library { get; }

        public DependencyClassification Classification { get; set; }

        public override string ToString()
        {
            return $"Library: {Library.Name}, Classification: {Classification}";
        }
    }
}
