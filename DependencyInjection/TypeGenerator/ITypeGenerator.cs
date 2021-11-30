using System;
using System.Collections.Generic;
using DependencyInjection.Config;
using DependencyInjection.Implementations;

namespace DependencyInjection.TypeGenerator
{
    public interface ITypeGenerator
    {
        object Generate(Implementation implementation, Type genericParamType = null);

        IEnumerable<object> GenerateCollection(ICollection<Implementation> implementations, Type dependencyType,
            Type genericParamType = null);
    }
}