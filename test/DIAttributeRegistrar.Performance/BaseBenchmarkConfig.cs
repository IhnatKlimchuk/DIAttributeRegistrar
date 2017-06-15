using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Validators;

namespace DIAttributeRegistrar.Performance
{
    public class BaseBenchmarkConfig : ManualConfig
    {
        public BaseBenchmarkConfig()
        {
            Add(JitOptimizationsValidator.FailOnError);
            Add(MemoryDiagnoser.Default);
            Add(StatisticColumn.OperationsPerSecond);

            Add(Job.Default
                .With(BenchmarkDotNet.Environments.Runtime.Core)
                .WithRemoveOutliers(false)
                .With(RunStrategy.Throughput)
                .WithLaunchCount(3)
                .WithWarmupCount(5)
                .WithTargetCount(10));
        }
    }
}
