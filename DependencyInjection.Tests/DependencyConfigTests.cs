using System;
using System.Collections.Generic;
using DependencyInjection.Config;
using DependencyInjection.Implementations;
using DependencyInjection.Tests.TestTypes.Abstract;
using DependencyInjection.Tests.TestTypes.Classes;
using DependencyInjection.Tests.TestTypes.Interfaces;
using Xunit;

namespace DependencyInjection.Tests
{
    public class DependencyConfigTests
    {
        [Fact]
        public void Register_ImplementationTypeIsAbstractOrInterface_ThrowsArgumentException()
        {
            var config = new DependenciesConfig();

            Assert.Throws<ArgumentException>(() => 
                config.Register(typeof(ITestInterface), typeof(ITestInterface)));
        }
        
        [Fact]
        public void Register_ImplementationTypeWithoutAccessibleConstructor_ThrowsArgumentException()
        {
            var config = new DependenciesConfig();

            Assert.Throws<ArgumentException>(() => 
                config.Register(typeof(ITestInterface), typeof(TestClass1)));
        }
        
        [Fact]
        public void Register_TypesNotAssignable_ThrowsArgumentException()
        {
            var config = new DependenciesConfig();

            Assert.Throws<ArgumentException>(() => 
                config.Register(typeof(ITestInterface), typeof(TestAbstractClassImpl1)));
        }
        
        [Fact]
        public void Register_GenericTypesNotAssignable_ThrowsArgumentException()
        {
            var config = new DependenciesConfig();

            Assert.Throws<ArgumentException>(() =>
                config.Register(typeof(ITestGenericCollection<int>), 
                    typeof(TestAbstractGenericClassImpl1<int>)));
        }
        
        [Fact]
        public void Register_OpenGenericTypesNotAssignable_ThrowsArgumentException()
        {
            var config = new DependenciesConfig();

            Assert.Throws<ArgumentException>(() =>
                config.Register(typeof(ITestGenericCollection<>), typeof(TestAbstractGenericClassImpl1<>)));
        }
        
        [Fact]
        public void Register_OnlyOneTypeGeneric_ThrowsArgumentException()
        {
            var config = new DependenciesConfig();

            Assert.Throws<ArgumentException>(() => 
                config.Register(typeof(TestAbstractClass), typeof(TestAbstractGenericClass<>)));
        }

        [Fact]
        public void Register_OpenGenericTypeAndSingletonLifetime_ThrowsArgumentException()
        {
            var config = new DependenciesConfig();

            Assert.Throws<ArgumentException>(() =>
                config.Register(typeof(ITestGenericCollection<>), typeof(TestGenericCollection1<>),
                    Lifetime.Singleton));
        }
        
        [Fact]
        public void Register_CorrectSingleAddition_AddedSingleCorrectImplementation()
        {
            var config = new DependenciesConfig();
            var expectedImplementation =
                new PerInstanceImplementation(null, typeof(TestClass2));

            config.Register(typeof(ITestInterface), typeof(TestClass2));

            Assert.Single(config.Implementations);
            Assert.Contains(config.Implementations, pair => pair.Key == typeof(ITestInterface));
            Assert.Single(config.Implementations[typeof(ITestInterface)]);
            Assert.Contains(config.Implementations[typeof(ITestInterface)], 
                implementation => implementation.Lifetime == expectedImplementation.Lifetime && 
                                  implementation.Name == expectedImplementation.Name &&
                                  implementation.Type == expectedImplementation.Type);
        }

        [Fact]
        public void Register_CorrectDoubleAdditionOfSameDependencyType_AddedAllImplementations()
        {
            var config = new DependenciesConfig();
            var expectedImplementations = new List<Implementation>()
            {
                new PerInstanceImplementation(null, typeof(TestClass2)),
                new PerInstanceImplementation(null, typeof(TestClass3)),
            };

            config.Register(typeof(ITestInterface), typeof(TestClass2));
            config.Register(typeof(ITestInterface), typeof(TestClass3));

            Assert.Single(config.Implementations);
            Assert.Contains(config.Implementations, pair => pair.Key == typeof(ITestInterface));
            Assert.True(config.Implementations[typeof(ITestInterface)].Count == 2);
            
            foreach (var expectedImplementation in expectedImplementations)
            {
                Assert.Contains(config.Implementations[typeof(ITestInterface)],
                    implementation => implementation.Lifetime == expectedImplementation.Lifetime &&
                                      implementation.Name == expectedImplementation.Name &&
                                      implementation.Type == expectedImplementation.Type);
            }
        }
        
        [Fact]
        public void Register_CorrectSelfAddition_AddedCorrectSingleImplementation()
        {
            var config = new DependenciesConfig();
            var expectedImplementations = new List<Implementation>()
            {
                new PerInstanceImplementation(null, typeof(TestClass2)),
            };

            config.Register(typeof(TestClass2), typeof(TestClass2));

            Assert.Single(config.Implementations);
            Assert.Contains(config.Implementations, pair => pair.Key == typeof(TestClass2));
            Assert.Single(config.Implementations[typeof(TestClass2)]);
            
            foreach (var expectedImplementation in expectedImplementations)
            {
                Assert.Contains(config.Implementations[typeof(TestClass2)],
                    implementation => implementation.Lifetime == expectedImplementation.Lifetime &&
                                      implementation.Name == expectedImplementation.Name &&
                                      implementation.Type == expectedImplementation.Type);
            }
        }
        
        [Fact]
        public void Register_CorrectSingleAdditionWithSingletonLifetime_AddedCorrectSingleImplementation()
        {
            var config = new DependenciesConfig();
            var expectedImplementations = new List<Implementation>()
            {
                new SingletonImplementation(null, typeof(TestClass2)),
            };

            config.Register(typeof(TestClass2), typeof(TestClass2), Lifetime.Singleton);

            Assert.Single(config.Implementations);
            Assert.Contains(config.Implementations, pair => pair.Key == typeof(TestClass2));
            Assert.Single(config.Implementations[typeof(TestClass2)]);
            
            foreach (var expectedImplementation in expectedImplementations)
            {
                Assert.Contains(config.Implementations[typeof(TestClass2)],
                    implementation => implementation.Lifetime == expectedImplementation.Lifetime &&
                                      implementation.Name == expectedImplementation.Name &&
                                      implementation.Type == expectedImplementation.Type);
            }
        }
        
        [Fact]
        public void Register_CorrectNamedSingleAddition_AddedCorrectSingleImplementation()
        {
            var config = new DependenciesConfig();
            var expectedImplementations = new List<Implementation>()
            {
                new PerInstanceImplementation("name", typeof(TestClass2)),
            };

            config.Register(typeof(TestClass2), typeof(TestClass2), Lifetime.PerInstance, "name");

            Assert.Single(config.Implementations);
            Assert.Contains(config.Implementations, pair => pair.Key == typeof(TestClass2));
            Assert.Single(config.Implementations[typeof(TestClass2)]);
            
            foreach (var expectedImplementation in expectedImplementations)
            {
                Assert.Contains(config.Implementations[typeof(TestClass2)],
                    implementation => implementation.Lifetime == expectedImplementation.Lifetime &&
                                      implementation.Name == expectedImplementation.Name &&
                                      implementation.Type == expectedImplementation.Type);
            }
        }
    }
}