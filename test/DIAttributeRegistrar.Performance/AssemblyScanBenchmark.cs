using BenchmarkDotNet.Attributes;
using DIAttributeRegistrar.AssemblyDiscovery;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DIAttributeRegistrar.Performance
{
    [Config(typeof(BaseBenchmarkConfig))]
    public class AssemblyScanBenchmark
    {
        private const int OperationsPerInvoke = 5000;
        
        public void GlobalSetup()
        {
        }
        
        [Benchmark(Baseline = true, OperationsPerInvoke = OperationsPerInvoke)]
        public void WithSpecifiedAssemblies()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            IEnumerable<Assembly> assemblies = Enumerable.Repeat(this.GetType().GetTypeInfo().Assembly, 1);
            AttributeRegistrar attributeRegistrar = new AttributeRegistrar();

            for (int i = 0; i < OperationsPerInvoke; i++)
            {
                attributeRegistrar.RegisterTypes(serviceCollection, assemblies, null);
                serviceCollection.BuildServiceProvider();
            }
        }

        [Benchmark(OperationsPerInvoke = OperationsPerInvoke)]
        public void WithFullAssembliesScan()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            AttributeRegistrar attributeRegistrar = new AttributeRegistrar(new FullScanAssemblyDiscoveryProvider());
            
            for (int i = 0; i < OperationsPerInvoke; i++)
            {
                attributeRegistrar.RegisterTypes(serviceCollection, null);
                serviceCollection.BuildServiceProvider();
            }
        }

        [Benchmark(OperationsPerInvoke = OperationsPerInvoke)]
        public void WithSimpleAssembliesScan()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            AttributeRegistrar attributeRegistrar = new AttributeRegistrar(new SimpleScanAssemblyDiscoveryProvider());

            for (int i = 0; i < OperationsPerInvoke; i++)
            {
                attributeRegistrar.RegisterTypes(serviceCollection, null);
                serviceCollection.BuildServiceProvider();
            }

        }

        [Benchmark(OperationsPerInvoke = OperationsPerInvoke)]
        public void WithOptimizedAssembliesScan()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            AttributeRegistrar attributeRegistrar = new AttributeRegistrar(new DefaultAssemblyDiscoveryProvider());

            for (int i = 0; i < OperationsPerInvoke; i++)
            {
                attributeRegistrar.RegisterTypes(serviceCollection, null);
                serviceCollection.BuildServiceProvider();
            }
        }

        [Register]
        private class AssemblyScanBenchmark_TestClassA
        {
        }

        internal class FullScanAssemblyDiscoveryProvider : IAssemblyDiscoveryProvider
        {
            public IEnumerable<Assembly> GetCandidateAssemblies()
            {
                return DependencyContext
                    .Default
                    .RuntimeLibraries
                    .SelectMany(l => l.GetDefaultAssemblyNames(DependencyContext.Default))
                    .Select(Assembly.Load);
            }
        }

        internal class SimpleScanAssemblyDiscoveryProvider : IAssemblyDiscoveryProvider
        {
            public IEnumerable<Assembly> GetCandidateAssemblies()
            {
                var currentAssemblyFullName = this.GetType().GetTypeInfo().Assembly.FullName;
                return DependencyContext
                    .Default
                    .RuntimeLibraries
                    .SelectMany(l => l.GetDefaultAssemblyNames(DependencyContext.Default))
                    .Select(Assembly.Load)
                    .Where(t =>
                        t.GetReferencedAssemblies()
                            .Select(Assembly.Load)
                            .Any(ra => 0 == String.Compare(ra.FullName, currentAssemblyFullName, StringComparison.OrdinalIgnoreCase)));
            }
        }
    }
}