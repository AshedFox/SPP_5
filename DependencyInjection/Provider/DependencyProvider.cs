using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection.Config;
using DependencyInjection.Implementations;
using DependencyInjection.TypeGenerator;

namespace DependencyInjection.Provider
{
    public class DependencyProvider : IDependencyProvider
    {
        private readonly IDependenciesConfig _config;
        private readonly ITypeGenerator _typeGenerator;

        public DependencyProvider(IDependenciesConfig config)
        {
            _config = config;
            _typeGenerator = new TypeGenerator.TypeGenerator(config);
        }
        
        public TDependency Resolve<TDependency>(string name = null) where TDependency : class
        {
            return (TDependency)Resolve(typeof(TDependency), name);
        }

        private object Resolve(Type dependencyType, string name)
        {
            ICollection<Implementation> implementations;

            if (dependencyType.IsGenericType && dependencyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var argumentType = dependencyType.GetGenericArguments()[0];

                _config.Implementations.TryGetValue(argumentType, out implementations);
                
                if (argumentType.IsGenericType && implementations is null)
                {
                    _config.Implementations.TryGetValue(argumentType.GetGenericTypeDefinition(), out implementations);
                }

                if (implementations is null)
                {
                    throw new ArgumentException("Dependency type don't have any implementations");
                }

                return dependencyType.IsGenericType
                    ? _typeGenerator.GenerateCollection(implementations, argumentType,
                        argumentType.GetGenericArguments()[0])
                    : _typeGenerator.GenerateCollection(implementations, argumentType);
            }
            else
            {
                _config.Implementations.TryGetValue(dependencyType, out implementations);

                if (dependencyType.IsGenericType && implementations is null)
                {
                    _config.Implementations.TryGetValue(dependencyType.GetGenericTypeDefinition(), out implementations);
                }

                if (implementations is null)
                {
                    throw new ArgumentException("Dependency type don't have any implementations");
                }

                var implementation = name is null
                    ? implementations.FirstOrDefault()
                    : implementations.FirstOrDefault(implementation => implementation.Name == name);

                if (implementation is null)
                {
                    throw new ArgumentException("Dependency type don't have necessary named implementation");
                }

                return dependencyType.IsGenericType
                    ? _typeGenerator.Generate(implementation, dependencyType.GetGenericArguments()[0])
                    : _typeGenerator.Generate(implementation);
            }
        }
    }
}