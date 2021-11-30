using System;
using DependencyInjection.Config;
using DependencyInjection.TypeGenerator;

namespace DependencyInjection.Implementations
{
    public class PerInstanceImplementation: Implementation
    {
        public PerInstanceImplementation(string name, Type type) : base(name, type, Lifetime.PerInstance)
        { }

        public override object GetInstance(ITypeGenerator generator)
        {
            return generator.Generate(this);
        }
    }
}