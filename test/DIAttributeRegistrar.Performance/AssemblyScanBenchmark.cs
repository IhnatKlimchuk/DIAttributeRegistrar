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
        private const int OperationsPerInvoke = 100;
        
        public void GlobalSetup()
        {
        }
        
        [Benchmark(Baseline = true, OperationsPerInvoke = OperationsPerInvoke)]
        public void WithSpecifiedAssemblies()
        {
            Func<IEnumerable<Assembly>> getSpecifiedAssemblies = () =>
            {
                return Enumerable.Repeat(this.GetType().GetTypeInfo().Assembly, 1);
            };
            
            for (int i = 0; i < OperationsPerInvoke; i++)
            {
                IServiceCollection serviceCollection = new ServiceCollection();
                AttributeRegistrar attributeRegistrar = new AttributeRegistrar(getSpecifiedAssemblies);
                attributeRegistrar.RegisterTypes(serviceCollection, null);
                serviceCollection.BuildServiceProvider();
            }
        }

        [Benchmark(OperationsPerInvoke = OperationsPerInvoke)]
        public void GoingThroughAllAssemblies()
        {
            Func<IEnumerable<Assembly>> getAllAssemblies = () =>
            {
                return DependencyContext
                    .Default
                    .RuntimeLibraries
                    .SelectMany(l => l.GetDefaultAssemblyNames(DependencyContext.Default))
                    .Select(Assembly.Load);
            };

            for (int i = 0; i < OperationsPerInvoke; i++)
            {
                IServiceCollection serviceCollection = new ServiceCollection();
                AttributeRegistrar attributeRegistrar = new AttributeRegistrar(getAllAssemblies);
                attributeRegistrar.RegisterTypes(serviceCollection, null);
                serviceCollection.BuildServiceProvider();
            }

        }

        [Benchmark(OperationsPerInvoke = OperationsPerInvoke)]
        public void GoingThroughAssembliesWithReferenceCheck()
        {
            Func<IEnumerable<Assembly>> getAssembliesWithReference = () =>
            {
                var currentAssemblyFullName = this.GetType().GetTypeInfo().Assembly.GetName().FullName;
                return DependencyContext
                    .Default
                    .RuntimeLibraries
                    .SelectMany(l => l.GetDefaultAssemblyNames(DependencyContext.Default))
                    .Select(Assembly.Load)
                    .Where(t =>
                        t.GetReferencedAssemblies()
                            .Any(ra => 0 == String.Compare(ra.FullName, currentAssemblyFullName, StringComparison.OrdinalIgnoreCase)));
            };

            for (int i = 0; i < OperationsPerInvoke; i++)
            {
                IServiceCollection serviceCollection = new ServiceCollection();
                AttributeRegistrar attributeRegistrar = new AttributeRegistrar(getAssembliesWithReference);
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