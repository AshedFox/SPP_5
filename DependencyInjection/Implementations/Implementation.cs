using System;
using DependencyInjection.Config;
using DependencyInjection.TypeGenerator;

namespace DependencyInjection.Implementations
{
    public abstract class Implementation
    {
        protected Implementation(string name, Type type, Lifetime lifetime)
        {
            Name = name;
            Type = type;
            Lifetime = lifetime;
        }

        public string Name { get; }
        public Type Type { get; }
        public Lifetime Lifetime { get; }

        public abstract object GetInstance(ITypeGenerator generator);
    }
}