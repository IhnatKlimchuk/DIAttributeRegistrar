using BenchmarkDotNet.Running;
using System;
using System.Reflection;

namespace DIAttributeRegistrar.Performance
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).GetTypeInfo().Assembly).Run(args);
            Console.Read();
        }
    }
}