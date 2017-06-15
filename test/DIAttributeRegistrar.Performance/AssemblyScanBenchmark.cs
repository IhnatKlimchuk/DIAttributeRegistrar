using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
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
            IEnumerable<Assembly> assembliesToSearch = Enumerable.Repeat(this.GetType().GetTypeInfo().Assembly, 1);
            AttributeRegistrar attributeRegistrar = new AttributeRegistrar();

            for (int i = 0; i < OperationsPerInvoke; i++)
            {
                attributeRegistrar.RegisterTypes(serviceCollection, assembliesToSearch, null);
                serviceCollection.BuildServiceProvider();
            }
            
        }
        
        [Register]
        private class AssemblyScanBenchmark_TestClassA
        {
        }
    }
}