using BenchmarkDotNet.Attributes;
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
        private const int OperationsPerInvoke = 50000;
        
        [GlobalSetup]
        public void GlobalSetup()
        {
        }
        
        [Benchmark(Baseline = true, OperationsPerInvoke = OperationsPerInvoke)]
        public void WithSpecifiedAssemblies()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            Func<IEnumerable<Assembly>> getSpecifiedAssemblies = () =>
            {
                return Enumerable.Repeat(this.GetType().GetTypeInfo().Assembly, 1);
            };
            AttributeRegistrar attributeRegistrar = new AttributeRegistrar(getSpecifiedAssemblies);

            for (int i = 0; i < OperationsPerInvoke; i++)
            {
                attributeRegistrar.RegisterTypes(serviceCollection, null);
                serviceCollection.BuildServiceProvider();
            }
            
        }

        [Benchmark(OperationsPerInvoke = OperationsPerInvoke)]
        public void GoingThroughAllAssemblies()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            Func<IEnumerable<Assembly>> getAllAssemblies = () =>
            {
                return DependencyContext
                    .Default
                    .RuntimeLibraries
                    .SelectMany(l => l.GetDefaultAssemblyNames(DependencyContext.Default))
                    .Select(Assembly.Load);
            };
            AttributeRegistrar attributeRegistrar = new AttributeRegistrar(getAllAssemblies);

            for (int i = 0; i < OperationsPerInvoke; i++)
            {
                attributeRegistrar.RegisterTypes(serviceCollection, null);
                serviceCollection.BuildServiceProvider();
            }

        }

        [Benchmark(OperationsPerInvoke = OperationsPerInvoke)]
        public void GoingThroughAssembliesWithReference()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            Func<IEnumerable<Assembly>> getAssembliesWithReference = () =>
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
            };
            AttributeRegistrar attributeRegistrar = new AttributeRegistrar(getAssembliesWithReference);

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
    }
}