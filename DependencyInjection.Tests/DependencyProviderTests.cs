using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection.Config;
using DependencyInjection.Implementations;
using DependencyInjection.Provider;
using DependencyInjection.Tests.TestTypes.Abstract;
using DependencyInjection.Tests.TestTypes.Classes;
using DependencyInjection.Tests.TestTypes.Interfaces;
using DependencyInjection.TypeGenerator;
using FluentAssertions;
using Moq;
using Xunit;

namespace DependencyInjection.Tests
{
    public class DependencyProviderTests
    {
        [Fact]
        public void Resolve_NoImplementations_ThrowsArgumentException()
        {
            var configMock = new Mock<IDependenciesConfig>();
            configMock.Setup(config => config.Implementations)
                .Returns(() => new Dictionary<Type, ICollection<Implementation>>());
            
            var provider = new DependencyProvider(configMock.Object);

            Assert.Throws<ArgumentException>(() => provider.Resolve<ITestInterface>());

            //var a = provider.Resolve<ICollection<float>>();
            //var b = provider.Resolve<ICollection<List<int>>>();
            //var c = provider.Resolve<ITypeGenerator>();
            //var d = provider.Resolve<IEnumerable<ICollection<int>>>();
        }
        
        [Fact]
        public void Resolve_NoImplementationsGeneric_ThrowsArgumentException()
        {
            var configMock = new Mock<IDependenciesConfig>();
            configMock.Setup(config => config.Implementations)
                .Returns(() => new Dictionary<Type, ICollection<Implementation>>());
            var provider = new DependencyProvider(configMock.Object);

            Assert.Throws<ArgumentException>(() => provider.Resolve<ITestGenericCollection<int>>());
        }
        
        [Fact]
        public void Resolve_NoImplementationsCollection_ThrowsArgumentException()
        {
            var configMock = new Mock<IDependenciesConfig>();
            configMock.Setup(config => config.Implementations)
                .Returns(() => new Dictionary<Type, ICollection<Implementation>>());
            var provider = new DependencyProvider(configMock.Object);
            
            Assert.Throws<ArgumentException>(() => provider.Resolve<IEnumerable<ITestInterface>>());
        }
        
        [Fact]
        public void Resolve_NoNamedImplementation_ThrowsArgumentException()
        {
            var configMock = new Mock<IDependenciesConfig>();
            configMock.Setup(config => config.Implementations)
                .Returns(() => new Dictionary<Type, ICollection<Implementation>>()
                {
                    {
                        typeof(ITestInterface), 
                        new List<Implementation>() 
                        { 
                            new PerInstanceImplementation(null, typeof(TestClass2)),
                            new PerInstanceImplementation("name", typeof(TestClass3)),
                        } 
                    }
                });
            var provider = new DependencyProvider(configMock.Object);
            
            Assert.Throws<ArgumentException>(() => provider.Resolve<ITestInterface>("12"));
        }
        
        [Fact]
        public void Resolve_ConfigHasNecessaryImplementation_ReturnsCorrectObject()
        {
            var configMock = new Mock<IDependenciesConfig>();
            configMock.Setup(config => config.Implementations)
                .Returns(() => new Dictionary<Type, ICollection<Implementation>>()
                {
                    {
                        typeof(ITestInterface), 
                        new List<Implementation>() 
                        { 
                            new PerInstanceImplementation(null, typeof(TestClass2)),
                            new PerInstanceImplementation("name", typeof(TestClass3)),
                        } 
                    }
                });
            var provider = new DependencyProvider(configMock.Object);
            
            var actual = provider.Resolve<ITestInterface>();

            Assert.NotNull(actual);
            Assert.IsAssignableFrom<ITestInterface>(actual);
        }
        
        [Fact]
        public void Resolve_ConfigHasNecessaryNamedImplementation_ReturnsCorrectObject()
        {
            var configMock = new Mock<IDependenciesConfig>();
            configMock.Setup(config => config.Implementations)
                .Returns(() => new Dictionary<Type, ICollection<Implementation>>()
                {
                    {
                        typeof(ITestInterface), 
                        new List<Implementation>() 
                        { 
                            new PerInstanceImplementation(null, typeof(TestClass2)),
                            new PerInstanceImplementation("name", typeof(TestClass3)),
                        } 
                    }
                });
            var provider = new DependencyProvider(configMock.Object);
            
            var actual = provider.Resolve<ITestInterface>("name");

            Assert.NotNull(actual);
            Assert.IsType<TestClass3>(actual);
        }
        
        [Fact]
        public void Resolve_GenericGeneration_ReturnsCorrectObject()
        {
            var configMock = new Mock<IDependenciesConfig>();
            configMock.Setup(config => config.Implementations)
                .Returns(() => new Dictionary<Type, ICollection<Implementation>>()
                {
                    {
                        typeof(ITestGenericCollection<int>), 
                        new List<Implementation>() 
                        { 
                            new PerInstanceImplementation(null, typeof(TestGenericCollection1<int>)),
                            new PerInstanceImplementation("name", typeof(TestGenericCollection2<int>)),
                        } 
                    }
                });
            var provider = new DependencyProvider(configMock.Object);
            
            var actual = provider.Resolve<ITestGenericCollection<int>>();

            Assert.NotNull(actual);
            Assert.IsAssignableFrom<ITestGenericCollection<int>>(actual);
        }
        
        [Fact]
        public void Resolve_GenericGenerationNamed_ReturnsCorrectObject()
        {
            var configMock = new Mock<IDependenciesConfig>();
            configMock.Setup(config => config.Implementations)
                .Returns(() => new Dictionary<Type, ICollection<Implementation>>()
                {
                    {
                        typeof(ITestGenericCollection<int>), 
                        new List<Implementation>() 
                        { 
                            new PerInstanceImplementation(null, typeof(TestGenericCollection1<int>)),
                            new PerInstanceImplementation("name", typeof(TestGenericCollection2<int>)),
                        } 
                    }
                });
            var provider = new DependencyProvider(configMock.Object);
            
            var actual = provider.Resolve<ITestGenericCollection<int>>("name");

            Assert.NotNull(actual);
            Assert.IsType<TestGenericCollection2<int>>(actual);
        }
        
        [Fact]
        public void Resolve_GenericGenerationFromOpenGenerics_ReturnsCorrectObject()
        {
            var configMock = new Mock<IDependenciesConfig>();
            configMock.Setup(config => config.Implementations)
                .Returns(() => new Dictionary<Type, ICollection<Implementation>>()
                {
                    {
                        typeof(ITestGenericCollection<>), 
                        new List<Implementation>() 
                        { 
                            new PerInstanceImplementation(null, typeof(TestGenericCollection1<>)),
                            new PerInstanceImplementation("name", typeof(TestGenericCollection2<>)),
                        } 
                    }
                });
            var provider = new DependencyProvider(configMock.Object);
            
            var actual = provider.Resolve<ITestGenericCollection<int>>();

            Assert.NotNull(actual);
            Assert.IsAssignableFrom<ITestGenericCollection<int>>(actual);
        }
        
        [Fact]
        public void Resolve_GenericGenerationFromOpenGenericsNamed_ReturnsCorrectObject()
        {
            var configMock = new Mock<IDependenciesConfig>();
            configMock.Setup(config => config.Implementations)
                .Returns(() => new Dictionary<Type, ICollection<Implementation>>()
                {
                    {
                        typeof(ITestGenericCollection<>), 
                        new List<Implementation>() 
                        { 
                            new PerInstanceImplementation(null, typeof(TestGenericCollection1<>)),
                            new PerInstanceImplementation("name", typeof(TestGenericCollection2<>)),
                        } 
                    }
                });
            var provider = new DependencyProvider(configMock.Object);
            
            var actual = provider.Resolve<ITestGenericCollection<int>>("name");

            Assert.NotNull(actual);
            Assert.IsType<TestGenericCollection2<int>>(actual);
        }
        
        [Fact]
        public void Resolve_GeneratingClassWithDependencyKeyAttribute_ReturnsCorrectObject()
        {
            var configMock = new Mock<IDependenciesConfig>();
            configMock.Setup(config => config.Implementations)
                .Returns(() => new Dictionary<Type, ICollection<Implementation>>()
                {
                    {
                        typeof(ITestInterface), 
                        new List<Implementation>() 
                        { 
                            new PerInstanceImplementation(null, typeof(TestClass2)),
                            new PerInstanceImplementation("name", typeof(TestClass3)),
                        } 
                    },
                    {
                        typeof(TestAbstractGenericClass<>), 
                        new List<Implementation>() 
                        { 
                            new PerInstanceImplementation(null, typeof(TestAbstractGenericClassImpl1<>)),
                            new PerInstanceImplementation(null, typeof(TestAbstractGenericClassImpl2<>)),
                        } 
                    },
                });
            var provider = new DependencyProvider(configMock.Object);
            
            var actual = provider.Resolve<TestAbstractGenericClass<int>>();

            Assert.NotNull(actual);
            Assert.IsType<TestClass3>(actual.Value);
        }
        
        [Fact]
        public void Resolve_GeneratingCollection_ReturnsCorrectCollection()
        {
            var configMock = new Mock<IDependenciesConfig>();
            configMock.Setup(config => config.Implementations)
                .Returns(() => new Dictionary<Type, ICollection<Implementation>>()
                {
                    {
                        typeof(ITestInterface), 
                        new List<Implementation>() 
                        { 
                            new PerInstanceImplementation(null, typeof(TestClass2)),
                            new PerInstanceImplementation("name", typeof(TestClass3)),
                        } 
                    },
                    {
                        typeof(TestAbstractGenericClass<>), 
                        new List<Implementation>() 
                        { 
                            new PerInstanceImplementation(null, typeof(TestAbstractGenericClassImpl1<>)),
                            new PerInstanceImplementation(null, typeof(TestAbstractGenericClassImpl2<>)),
                        } 
                    },
                });
            var provider = new DependencyProvider(configMock.Object);
            
            var actual = provider.Resolve<IEnumerable<TestAbstractGenericClass<int>>>().ToList();

            Assert.NotNull(actual);
            Assert.True(actual.Count == 2);
            actual.Should().ContainItemsAssignableTo<TestAbstractGenericClass<int>>();
        }
    }
}