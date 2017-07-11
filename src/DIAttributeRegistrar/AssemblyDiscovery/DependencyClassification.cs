namespace DIAttributeRegistrar.AssemblyDiscovery
{
    internal enum DependencyClassification
    {
        Unknown = 0,
        ReferencesAttributeRegistrar = 1,
        DoesNotReferenceAttributeRegistrar = 2,
        AttributeRegistrarReference = 3,
    }
}
