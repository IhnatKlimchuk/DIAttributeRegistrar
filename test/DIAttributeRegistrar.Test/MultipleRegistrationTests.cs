using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DIAttributeRegistrar.Test
{
    public class MultipleRegistrationTests : TestBase
    {
        public MultipleRegistrationTests() : base()
        {
            
        }

        [Fact]
        public void TestClassWithMultipleAttributes_WithDifferentLifetime_Registered_WithMinimalLifetime()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies)
                .BuildServiceProvider();

            var firstTestClass = serviceProvider.GetService<TestClassA>();
            var secondTestClass = serviceProvider.GetService<TestClassA>();
            Assert.NotSame(firstTestClass, secondTestClass);
        }

        [Fact]
        public void InverseTestClassWithMultipleAttributes_WithDifferentLifetime_Registered_WithMinimalLifetime()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies)
                .BuildServiceProvider();

            var firstTestClass = serviceProvider.GetService<InverseTestClassA>();
            var secondTestClass = serviceProvider.GetService<InverseTestClassA>();
            Assert.NotSame(firstTestClass, secondTestClass);
        }

        [Register(ServiceLifetime.Singleton)]
        [Register(ServiceLifetime.Scoped)]
        [Register(ServiceLifetime.Transient)]
        public class TestClassA
        {

        }
        
        [Register(ServiceLifetime.Transient)]
        [Register(ServiceLifetime.Scoped)]
        [Register(ServiceLifetime.Singleton)]
        public class InverseTestClassA
        {

        }
    }
}
