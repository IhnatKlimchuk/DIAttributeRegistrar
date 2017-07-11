using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DIAttributeRegistrar.Tests
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

            var firstTestClass = serviceProvider.GetService<MultipleRegistrationTests_TestClassA>();
            var secondTestClass = serviceProvider.GetService<MultipleRegistrationTests_TestClassA>();
            Assert.NotSame(firstTestClass, secondTestClass);
        }

        [Fact]
        public void InverseTestClassWithMultipleAttributes_WithDifferentLifetime_Registered_WithMinimalLifetime()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies)
                .BuildServiceProvider();

            var firstTestClass = serviceProvider.GetService<MultipleRegistrationTests_InverseTestClassA>();
            var secondTestClass = serviceProvider.GetService<MultipleRegistrationTests_InverseTestClassA>();
            Assert.NotSame(firstTestClass, secondTestClass);
        }

        [Fact]
        public void TestClassWithMultipleAttributes_WithDifferentLifetime_Registered_WithCorrectTag()
        {
            var devServiceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies, Constans.DevTag)
                .BuildServiceProvider();
            
            var firstDevTestClass = devServiceProvider.GetService<MultipleRegistrationTests_TestClassB>();
            var secondDevTestClass = devServiceProvider.GetService<MultipleRegistrationTests_TestClassB>();
            Assert.Same(firstDevTestClass, secondDevTestClass);

            var testServiceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies, Constans.TestTag)
                .BuildServiceProvider();

            var firstTestTestClass = testServiceProvider.GetService<MultipleRegistrationTests_TestClassB>();
            var secondtestTestClass = testServiceProvider.GetService<MultipleRegistrationTests_TestClassB>();
            Assert.NotSame(firstTestTestClass, secondtestTestClass);
        }

        [Fact]
        public void TestClassWithMultipleAttributes_WithDifferentTags_Registered()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies, Constans.LiveTag)
                .BuildServiceProvider();

            var testClass = serviceProvider.GetService<MultipleRegistrationTests_TestClassC>();
        }

        [Register(ServiceLifetime.Singleton)]
        [Register(ServiceLifetime.Scoped)]
        [Register(ServiceLifetime.Transient)]
        public class MultipleRegistrationTests_TestClassA
        {

        }
        
        [Register(ServiceLifetime.Transient)]
        [Register(ServiceLifetime.Scoped)]
        [Register(ServiceLifetime.Singleton)]
        public class MultipleRegistrationTests_InverseTestClassA
        {

        }
        
        [Register(ServiceLifetime.Singleton, Constans.DevTag)]
        [Register(ServiceLifetime.Transient, Constans.TestTag)]
        public class MultipleRegistrationTests_TestClassB
        {

        }

        [Register()]
        [Register(Constans.LiveTag)]
        public class MultipleRegistrationTests_TestClassC
        {

        }
    }
}
