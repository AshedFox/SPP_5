using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection.Extensions;
using DependencyInjection.Implementations;

namespace DependencyInjection.Config
{
    public class DependenciesConfig: IDependenciesConfig
    {
        private readonly Dictionary<Type, ICollection<Implementation>> _implementations = new();
        public IDictionary<Type, ICollection<Implementation>> Implementations => _implementations;
        
        public void Register<TDependency, TImplementation>(Lifetime lifetime = Lifetime.PerInstance, string name = null) 
            where TDependency : class 
            where TImplementation : TDependency
        {
            Register(typeof(TDependency), typeof(TImplementation), lifetime, name);
        }

        public void Register(Type dependencyType, Type implementationType, Lifetime lifetime = Lifetime.PerInstance, 
            string name = null)
        {
            if (implementationType.IsInterface || implementationType.IsAbstract)
            {
                throw new ArgumentException("Implementation type can't be interface or abstract");
            }

            if (implementationType.GetConstructors().Length == 0)
            {
                throw new ArgumentException("Implementation type must have constructor");
            }
            
            if (dependencyType.IsGenericType ^ implementationType.IsGenericType)
            {
                throw new ArgumentException("Dependency type and implementation type should be both generic (or not generic)");
            }

            if (dependencyType.IsGenericTypeDefinition)
            {
                if (lifetime == Lifetime.Singleton)
                {
                    throw new ArgumentException("Open generic types can't be singleton");
                }
                
                if (!implementationType.IsAssignableToGenericType(dependencyType))
                {
                    throw new ArgumentException("Implementation type is not assignable to dependency type");
                }
            }
            else
            {
                if (!implementationType.IsAssignableTo(dependencyType))
                {
                    throw new ArgumentException("Implementation type is not assignable to dependency type");
                }
            }

            Implementation implementation;
            
            if (lifetime == Lifetime.Singleton)
            {
                implementation = new SingletonImplementation(name, implementationType);
            }
            else
            {
                implementation = new PerInstanceImplementation(name, implementationType);
            }

            if (!_implementations.TryGetValue(dependencyType, out var implementations))
            {
                implementations = new List<Implementation>();

                _implementations.Add(dependencyType, implementations);
            }

            if (!implementations.ToList()
                .Exists(impl => impl.Name == implementation.Name && impl.Type == implementation.Type))
            {
                implementations.Add(implementation);
            }
        }
    }
}