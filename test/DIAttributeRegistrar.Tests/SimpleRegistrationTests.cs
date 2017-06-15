using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DIAttributeRegistrar.Test
{
    public class SimpleRegistrationTests : TestBase
    {
        public SimpleRegistrationTests() : base()
        {
            
        }

        [Fact]
        public void TestClassWithEmptyTags_Registered_WhenEmptyTagsRequired()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies)
                .BuildServiceProvider();

            var testClass = serviceProvider.GetService<SimpleRegistrationTests_TestClassA>();
            Assert.NotNull(testClass);
        }

        [Fact]
        public void TestClassWithEmptyTags_Registered_WhenDevTagRequired()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies, Constans.DevTag)
                .BuildServiceProvider();

            var testClass = serviceProvider.GetService<SimpleRegistrationTests_TestClassA>();
            Assert.NotNull(testClass);
        }

        [Fact]
        public void TestClassWithEmptyTags_Registered_WhenDevAndTestTagRequired()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies, Constans.DevTag, Constans.TestTag)
                .BuildServiceProvider();

            var testClass = serviceProvider.GetService<SimpleRegistrationTests_TestClassA>();
            Assert.NotNull(testClass);
        }

        [Fact]
        public void TestClassWithDevTag_Registered_WhenDevTagRequired()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies, Constans.DevTag)
                .BuildServiceProvider();

            var testClass = serviceProvider.GetService<SimpleRegistrationTests_TestClassB>();
            Assert.NotNull(testClass);
        }

        [Fact]
        public void TestClassWithDevTag_NotRegistered_WhenEmptyTagsRequired()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies)
                .BuildServiceProvider();

            var testClass = serviceProvider.GetService<SimpleRegistrationTests_TestClassB>();
            Assert.Null(testClass);
        }

        [Fact]
        public void TestClassWithDevTag_NotRegistered_WhenTestTagRequired()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies, Constans.TestTag)
                .BuildServiceProvider();

            var testClass = serviceProvider.GetService<SimpleRegistrationTests_TestClassB>();
            Assert.Null(testClass);
        }

        [Fact]
        public void TestClassWithDevTag_NotRegistered_WhenDevAndTestTagRequired()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies, Constans.DevTag, Constans.TestTag)
                .BuildServiceProvider();

            var testClass = serviceProvider.GetService<SimpleRegistrationTests_TestClassB>();
            Assert.Null(testClass);
        }

        [Fact]
        public void TestClassWithDevAndTestTags_NotRegistered_WhenEmptyTagsRequired()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies)
                .BuildServiceProvider();

            var testClass = serviceProvider.GetService<SimpleRegistrationTests_TestClassC>();
            Assert.Null(testClass);
        }

        [Fact]
        public void TestClassWithDevAndTestTags_Registered_WhenDevTagRequired()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies, Constans.DevTag)
                .BuildServiceProvider();

            var testClass = serviceProvider.GetService<SimpleRegistrationTests_TestClassC>();
            Assert.NotNull(testClass);
        }

        [Fact]
        public void TestClassWithDevAndTestTags_Registered_WhenDevAndTestTagsRequired()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies, Constans.DevTag, Constans.TestTag)
                .BuildServiceProvider();

            var testClass = serviceProvider.GetService<SimpleRegistrationTests_TestClassC>();
            Assert.NotNull(testClass);
        }

        [Fact]
        public void TestClassWithDevAndTestTags_NotRegistered_WhenLiveTagRequired()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies, Constans.LiveTag)
                .BuildServiceProvider();

            var testClass = serviceProvider.GetService<SimpleRegistrationTests_TestClassC>();
            Assert.Null(testClass);
        }

        [Fact]
        public void TestClassWithDevAndTestTags_NotRegistered_WhenTestAndLiveTagsRequired()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies, Constans.LiveTag, Constans.TestTag)
                .BuildServiceProvider();

            var testClass = serviceProvider.GetService<SimpleRegistrationTests_TestClassC>();
            Assert.Null(testClass);
        }

        [Register]
        public class SimpleRegistrationTests_TestClassA
        {

        }

        [Register(Constans.DevTag)]
        public class SimpleRegistrationTests_TestClassB
        {

        }

        [Register(Constans.DevTag, Constans.TestTag)]
        public class SimpleRegistrationTests_TestClassC
        {

        }

        [Register(Constans.DevTag, Constans.TestTag, Constans.LiveTag)]
        public class SimpleRegistrationTests_TestClassD
        {

        }
    }
}
