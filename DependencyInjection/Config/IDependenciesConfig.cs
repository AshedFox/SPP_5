using System;
using System.Collections.Generic;
using DependencyInjection.Implementations;

namespace DependencyInjection.Config
{
    public interface IDependenciesConfig
    {
        public IDictionary<Type, ICollection<Implementation>> Implementations { get; }
        
        void Register<TDependency, TImplementation>(Lifetime lifetime = Lifetime.PerInstance, string name = null)
            where TDependency : class
            where TImplementation : TDependency;

        void Register(Type dependencyType, Type implementationType, Lifetime lifetime = Lifetime.PerInstance, 
            string name = null);
    }
}