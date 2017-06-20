namespace DIAttributeRegistrar.AssemblyDiscovery
{
    internal enum DependencyClassification
    {
        Unknown = 0,
        ReferencesAttributeRegistar = 1,
        DoesNotReferenceAttributeRegistar = 2,
        AttributeRegistarReference = 3,
    }
}
