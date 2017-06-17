using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DIAttributeRegistrar.Test
{
    public class AsTypeRegistrationTests : TestBase
    {
        public AsTypeRegistrationTests() : base()
        {
            
        }

        [Fact]
        public void TestClassWithEmptyTags_Registered_PerAsType()
        {
            var serviceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies)
                .BuildServiceProvider();

            var baseTestClassFirst = serviceProvider.GetService<IBaseTestClassFirst>();
            var baseTestClassSecond = serviceProvider.GetService<IBaseTestClassSecond>();
            Assert.NotNull(baseTestClassFirst);
            Assert.NotNull(baseTestClassSecond);
            Assert.NotSame(baseTestClassFirst, baseTestClassSecond);
        }

        [Fact]
        public void TestClassesWithDifferentTags_Registered_AsType_PerTag()
        {
            var devServiceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies, Constans.DevTag)
                .BuildServiceProvider();

            var devBaseTestClassThird = devServiceProvider.GetService<IBaseTestClassThird>();
            Assert.NotNull(devBaseTestClassThird);
            Assert.IsType(typeof(AsTypeRegistrationTests_TestClassA), devBaseTestClassThird);

            var testServiceProvider = ServiceCollection
                .AddAttributeRegistration(CurrentAssemblies, Constans.TestTag)
                .BuildServiceProvider();

            var testBaseTestClassThird = testServiceProvider.GetService<IBaseTestClassThird>();
            Assert.NotNull(testBaseTestClassThird);
            Assert.IsType(typeof(AsTypeRegistrationTests_TestClassB), testBaseTestClassThird);
        }

        public interface IBaseTestClassFirst
        {

        }

        public interface IBaseTestClassSecond
        {

        }

        public interface IBaseTestClassThird
        {

        }

        [Register(typeof(IBaseTestClassFirst), ServiceLifetime.Singleton)]
        [Register(typeof(IBaseTestClassSecond), ServiceLifetime.Singleton)]
        public class AsTypeRegistrationTests_BaseTestClass : IBaseTestClassFirst, IBaseTestClassSecond, IBaseTestClassThird
        {

        }

        public interface ITestClassA
        {

        }

        [Register(typeof(IBaseTestClassThird), Constans.DevTag)]
        public class AsTypeRegistrationTests_TestClassA : AsTypeRegistrationTests_BaseTestClass, ITestClassA
        {

        }

        public interface ITestClassB
        {

        }

        [Register(typeof(IBaseTestClassThird), Constans.TestTag)]
        public class AsTypeRegistrationTests_TestClassB : AsTypeRegistrationTests_BaseTestClass, ITestClassB
        {

        }
    }
}
